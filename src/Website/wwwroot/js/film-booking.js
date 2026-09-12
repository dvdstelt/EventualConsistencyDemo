// Film booking flow: seat map -> confirmation.
// The confirmation is never faked: it stays "pending" until the back-end's real
// reply arrives over SignalR (OrderedRegularTicket). If the back-end endpoint is
// not running, the reply never comes and after CONFIRM_TIMEOUT_MS the panel
// degrades to "unconfirmed" - the command is safely on the queue, so the order is
// not lost, but the seats cannot be promised and the visitor is left hanging.
// That is the point of the demo: compare it with the raffle (film-raffle.js),
// which is designed to promise nothing more than an email later and therefore
// completes happily on a back-end that is down.
// The listener stays armed, so starting the back-end mid-demo flips the very same
// panel to "confirmed" without the visitor doing anything.
(function () {
  const flow = document.getElementById('flow');
  if (!flow) return;

  const root = document.getElementById('booking');
  const movieId = root.dataset.movieId;
  const movieTitle = root.dataset.movieTitle;
  const price = parseFloat(root.dataset.price || '0');

  const seatmap = document.getElementById('seatmap');
  const flowMeta = document.getElementById('flowMeta');
  const seatCountEl = document.getElementById('seatCount');
  const seatTotalEl = document.getElementById('seatTotal');
  const placeOrder = document.getElementById('placeOrder');

  const confirmPanel = document.getElementById('orderConfirm');
  const confirmTitle = document.getElementById('confirmTitle');
  const confirmText = document.getElementById('confirmText');
  const confirmSteps = [...document.querySelectorAll('#confirmStatus li')];
  const orderDetails = document.getElementById('orderDetails');
  const flowDone = document.getElementById('flowDone');

  // How long to wait for the back-end reply before admitting we cannot confirm.
  const CONFIRM_TIMEOUT_MS = 5000;

  let picked = new Set();
  let selection = null; // { theaterId, theaterName, time }

  // Only the submission that is currently on screen may update the panel: closing
  // the overlay (or starting another order) invalidates the previous one, so a late
  // reply is dropped instead of landing in the wrong confirmation.
  let currentOrder = 0;
  let awaitingReply = false;
  let unconfirmedTimer = null;
  let stepTimer = null;

  const money = (n) => n.toFixed(2).replace('.', ',');

  const XLINK = 'http://www.w3.org/1999/xlink';
  function seatSvg(state) {
    return '<svg class="seat-ico" viewBox="0 0 56 56"><use href="#seat-' + state +
           '" xlink:href="#seat-' + state + '"></use></svg>';
  }
  function setState(btn, state) {
    const use = btn.querySelector('use');
    use.setAttribute('href', '#seat-' + state);
    use.setAttributeNS(XLINK, 'href', '#seat-' + state);
  }

  // 6 rows x 10 cols with a few pre-taken seats.
  function buildSeats() {
    seatmap.innerHTML = '';
    const taken = new Set([2, 3, 14, 27, 28, 41, 55, 56, 57]);
    for (let i = 0; i < 60; i++) {
      const b = document.createElement('button');
      b.type = 'button';
      b.className = 'seat-cell';
      if (taken.has(i)) {
        b.classList.add('taken');
        b.disabled = true;
        b.innerHTML = seatSvg('taken');
      } else {
        b.innerHTML = seatSvg('normal');
        b.addEventListener('click', () => toggleSeat(b, i));
      }
      seatmap.appendChild(b);
    }
  }

  function toggleSeat(btn, i) {
    if (picked.has(i)) { picked.delete(i); btn.classList.remove('picked'); setState(btn, 'normal'); }
    else { picked.add(i); btn.classList.add('picked'); setState(btn, 'selected'); }
    const n = picked.size;
    seatCountEl.textContent = n;
    seatTotalEl.textContent = money(n * price);
    placeOrder.disabled = n === 0;
    placeOrder.textContent = n === 0 ? 'Confirm & pay' : 'Confirm & pay \u20ac' + money(n * price);
  }

  function openFlow(sel) {
    selection = sel;
    picked = new Set();
    buildSeats();
    seatCountEl.textContent = '0';
    seatTotalEl.textContent = '0,00';
    placeOrder.disabled = true;
    placeOrder.textContent = 'Confirm & pay';
    flowMeta.textContent = movieTitle + ' · ' + sel.theaterName + ' · ' + sel.time;
    resetConfirmation();
    showStep(1);
    flow.hidden = false;
    document.body.style.overflow = 'hidden';
  }

  function closeFlow() {
    // Walking away from an unconfirmed order abandons it as far as the screen is
    // concerned; the command keeps its own course in the back-end regardless.
    currentOrder++;
    awaitingReply = false;
    clearTimers();
    flow.hidden = true;
    document.body.style.overflow = '';
  }

  function clearTimers() {
    clearTimeout(unconfirmedTimer);
    clearInterval(stepTimer);
  }

  function resetConfirmation() {
    clearTimers();
    awaitingReply = false;
    orderDetails.innerHTML = '';
    confirmSteps.forEach(li => li.classList.remove('on'));
    flowDone.hidden = true;
    confirmPanel.dataset.state = 'pending';
    confirmTitle.textContent = 'Confirming your order\u2026';
    confirmText.textContent = "Hang on, we're reserving your seats with the ticketing service.";
  }

  // Light the remaining status lines one by one: with the reply in hand, the rest of
  // the back-end work (tickets issued, mail sent) really is under way.
  function revealRemainingSteps() {
    let i = confirmSteps.findIndex(li => !li.classList.contains('on'));
    if (i < 0) return;
    stepTimer = setInterval(() => {
      if (i >= confirmSteps.length) { clearInterval(stepTimer); return; }
      confirmSteps[i].classList.add('on');
      i++;
    }, 900);
  }

  function showStep(n) {
    flow.querySelectorAll('.flow-panel').forEach(p => { p.hidden = Number(p.dataset.panel) !== n; });
    flow.querySelectorAll('.fstep').forEach(s => {
      const step = Number(s.dataset.step);
      s.classList.toggle('active', step === n);
      s.classList.toggle('done', step < n);
    });
  }

  // Grey out today's showtimes whose start time has already passed; re-check
  // periodically so they drop out while the demo is running.
  function markPastShowtimes() {
    const now = new Date();
    const nowMinutes = now.getHours() * 60 + now.getMinutes();
    document.querySelectorAll('#cinemaList .st[data-day="0"][data-time]').forEach(st => {
      const [h, mn] = st.dataset.time.split(':').map(Number);
      if (h * 60 + mn <= nowMinutes) st.classList.add('past');
    });
  }
  markPastShowtimes();
  setInterval(markPastShowtimes, 30000);

  // Clickable date strip: each day shows its own (server-rendered, stable) set
  // of showtimes; picking a day reveals that day's buttons and hides the rest.
  // Days before a movie's premiere are disabled, so default to the first open one.
  const datePills = [...document.querySelectorAll('.date-strip .date-pill[data-day]')];
  if (datePills.length) {
    const showDay = (day) => {
      datePills.forEach(p => p.classList.toggle('active', p.dataset.day === String(day)));
      document.querySelectorAll('#cinemaList .st[data-day]').forEach(st => {
        st.hidden = st.dataset.day !== String(day);
      });
    };
    datePills.forEach(p => {
      if (!p.disabled) p.addEventListener('click', () => showDay(Number(p.dataset.day)));
    });
    const firstOpen = datePills.find(p => !p.disabled);
    if (firstOpen) showDay(Number(firstOpen.dataset.day));
  }

  // Wire every (available) showtime button to open the flow for its cinema/time.
  document.querySelectorAll('.st:not(.sold)').forEach(btn => {
    btn.addEventListener('click', () => {
      if (btn.classList.contains('past')) return;   // showing has already started
      const block = btn.closest('.cinema-block');
      openFlow({
        theaterId: block.dataset.theaterId,
        theaterName: block.querySelector('h3').textContent,
        time: btn.dataset.time
      });
    });
  });

  document.getElementById('flowClose').addEventListener('click', closeFlow);
  document.getElementById('flowDone').addEventListener('click', closeFlow);
  flow.addEventListener('click', (e) => { if (e.target === flow) closeFlow(); });

  placeOrder.addEventListener('click', () => {
    const order = ++currentOrder;
    resetConfirmation();
    awaitingReply = true;
    showStep(2);

    // Submit the real order over SignalR -> NServiceBus.
    registerMovieTicket({
      movieId: movieId,
      theatersContext_group: selection.theaterId,
      times_group: selection.time,
      numberOfTickets: picked.size
    }).then(() => {
      if (order !== currentOrder) return;
      // The website accepted the order and the command is on the queue. Whether the
      // back-end is there to handle it is a different question entirely.
      confirmSteps[0].classList.add('on');
      unconfirmedTimer = setTimeout(() => showUnconfirmed(order), CONFIRM_TIMEOUT_MS);
    }).catch(err => {
      console.error(err.toString());
      if (order !== currentOrder) return;
      awaitingReply = false;
      showUnreachable();
    });
  });

  function showUnconfirmed(order) {
    if (order !== currentOrder) return;
    confirmPanel.dataset.state = 'unconfirmed';
    confirmTitle.textContent = "We can't confirm your seats yet";
    confirmText.textContent = "Your order has been received and it won't get lost, but the ticketing service hasn't confirmed it yet. You'll be emailed as soon as your tickets are issued \u2014 please don't travel to the cinema until you have them.";
    flowDone.hidden = false;
  }

  function showUnreachable() {
    confirmPanel.dataset.state = 'unreachable';
    confirmTitle.textContent = "We couldn't place your order";
    confirmText.textContent = 'Nothing was reserved and nothing was charged. Please reload the page and try again.';
    flowDone.hidden = false;
  }

  // The real back-end reply, whenever it turns up: seconds later, or minutes later
  // because the endpoint was only just started.
  onRegularTicket(ticket => {
    if (!awaitingReply || flow.hidden) return;
    awaitingReply = false;
    clearTimers();
    confirmPanel.dataset.state = 'confirmed';
    confirmTitle.textContent = 'Thanks! Your tickets are on their way';
    confirmText.textContent = 'Your seats are confirmed. The e-tickets are emailed to you.';
    const tmpl = document.getElementById('regularticket');
    if (tmpl) { orderDetails.innerHTML = tmpl.innerHTML.supplant(ticket); }
    confirmSteps[0].classList.add('on');
    revealRemainingSteps();
    flowDone.hidden = false;
  });
})();

// Film booking flow: seat map -> payment -> confirmation.
// The confirmation deliberately reveals the post-payment work as separate
// asynchronous steps (payment received -> seats reserved -> e-tickets sent ->
// reminder scheduled). That is the visual for the messaging story. The actual
// order is submitted over SignalR (see registerMovieTicket in site.js), and the
// real server confirmation is rendered into #orderDetails when it returns.
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
  const payTotalEl = document.getElementById('payTotal');
  const toPay = document.getElementById('toPay');

  let picked = new Set();
  let selection = null; // { theaterId, theaterName, time }

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
    payTotalEl.textContent = money(n * price);
    toPay.disabled = n === 0;
  }

  function openFlow(sel) {
    selection = sel;
    picked = new Set();
    buildSeats();
    seatCountEl.textContent = '0';
    seatTotalEl.textContent = '0,00';
    toPay.disabled = true;
    flowMeta.textContent = movieTitle + ' · ' + sel.theaterName + ' · ' + sel.time;
    document.getElementById('orderDetails').innerHTML = '';
    showStep(1);
    flow.hidden = false;
    document.body.style.overflow = 'hidden';
  }

  function closeFlow() {
    flow.hidden = true;
    document.body.style.overflow = '';
  }

  function showStep(n) {
    flow.querySelectorAll('.flow-panel').forEach(p => { p.hidden = Number(p.dataset.panel) !== n; });
    flow.querySelectorAll('.fstep').forEach(s => {
      const step = Number(s.dataset.step);
      s.classList.toggle('active', step === n);
      s.classList.toggle('done', step < n);
    });
  }

  // Cinema search: filter the list by city or cinema name (case-insensitive
  // substring). data-search holds the lower-cased theater name, which contains
  // both, so "rott" matches every Rotterdam cinema.
  const cinemaSearch = document.getElementById('cinemaSearch');
  if (cinemaSearch) {
    const noCinemas = document.getElementById('noCinemas');
    const filter = () => {
      const q = cinemaSearch.value.trim().toLowerCase();
      let any = false;
      document.querySelectorAll('#cinemaList .cinema-block').forEach(block => {
        const match = block.dataset.search.includes(q);
        block.style.display = match ? '' : 'none';
        if (match) { any = true; }
      });
      if (noCinemas) { noCinemas.style.display = any ? 'none' : ''; }
    };
    cinemaSearch.addEventListener('input', filter);
    filter(); // apply the initial value ("Rotterdam")
  }

  // Wire every (available) showtime button to open the flow for its cinema/time.
  document.querySelectorAll('.st:not(.sold)').forEach(btn => {
    btn.addEventListener('click', () => {
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

  toPay.addEventListener('click', () => showStep(2));

  document.getElementById('doPay').addEventListener('click', () => {
    // Submit the real order over SignalR -> NServiceBus.
    registerMovieTicket({
      movieId: movieId,
      theatersContext_group: selection.theaterId,
      times_group: selection.time,
      numberOfTickets: picked.size
    });

    showStep(3);
    // Reveal the post-purchase async steps one by one, as independent messages
    // being processed after the payment is confirmed.
    const steps = document.querySelectorAll('#confirmStatus li');
    let i = 1; // first item ("Payment received") already .on
    const timer = setInterval(() => {
      if (i >= steps.length) { clearInterval(timer); return; }
      steps[i].classList.add('on');
      i++;
    }, 900);
  });
})();

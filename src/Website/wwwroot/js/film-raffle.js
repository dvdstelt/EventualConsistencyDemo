// Raffle sign-up flow (single logged-in user, so no login/email step).
// The confirmation reveals the post-registration work as separate asynchronous
// steps (received -> registered on account -> confirmation sent -> draw
// notification scheduled) as the visual for the messaging story. The actual
// entry is submitted over SignalR (registerMovieTicket in site.js); the real
// server confirmation is rendered into #raffleDetails when it returns.
(function () {
  const overlay = document.getElementById('raffle');
  if (!overlay) return;

  const root = document.getElementById('booking');
  const movieId = root.dataset.movieId;

  let selection = null; // { theaterId, time }

  function showStep(n) {
    overlay.querySelectorAll('.flow-panel').forEach(p => { p.hidden = Number(p.dataset.panel) !== n; });
    overlay.querySelectorAll('.fstep').forEach(s => {
      const step = Number(s.dataset.step);
      s.classList.toggle('active', step === n);
      s.classList.toggle('done', step < n);
    });
  }

  function open() {
    document.getElementById('raffleDetails').innerHTML = '';
    showStep(1);
    overlay.hidden = false;
    document.body.style.overflow = 'hidden';
  }
  function close() {
    overlay.hidden = true;
    document.body.style.overflow = '';
  }

  document.querySelectorAll('.st-raffle').forEach(btn => {
    btn.addEventListener('click', () => {
      const block = btn.closest('.cinema-block');
      selection = { theaterId: block.dataset.theaterId, time: btn.dataset.time };
      open();
    });
  });

  document.getElementById('raffleClose').addEventListener('click', close);
  document.getElementById('raffleDone').addEventListener('click', close);
  overlay.addEventListener('click', (e) => { if (e.target === overlay) close(); });

  document.getElementById('raffleSubmit').addEventListener('click', () => {
    // Submit the raffle entry over SignalR; the movie is a DrawingTicket, so the
    // hub responds with OrderedLotteryTicket.
    registerMovieTicket({
      movieId: movieId,
      theatersContext_group: selection.theaterId,
      times_group: selection.time,
      numberOfTickets: 1
    });

    showStep(2);
    const steps = overlay.querySelectorAll('#raffleStatus li');
    let i = 1; // first item ("Entry received") already .on
    const timer = setInterval(() => {
      if (i >= steps.length) { clearInterval(timer); return; }
      steps[i].classList.add('on');
      i++;
    }, 900);
  });
})();

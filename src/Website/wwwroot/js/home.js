// Header: transparent over the hero, solid once the page is scrolled.
(function () {
  const topbar = document.querySelector('.topbar');
  if (topbar) {
    const onScroll = function () {
      topbar.classList.toggle('solid', window.scrollY > 40);
    };
    window.addEventListener('scroll', onScroll, { passive: true });
    onScroll();
  }
})();

// Custom cinema dropdown: open/close panel, pick an option.
(function () {
  const select = document.getElementById('cinemaSelect');
  if (!select) return;
  const trigger = select.querySelector('.cs-trigger');
  const panel = select.querySelector('.cs-panel');
  const value = select.querySelector('.cs-value');
  const STORE_KEY = 'popcornCinema';

  function open() {
    panel.hidden = false;
    select.classList.add('open');
    trigger.setAttribute('aria-expanded', 'true');
  }
  function close() {
    panel.hidden = true;
    select.classList.remove('open');
    trigger.setAttribute('aria-expanded', 'false');
  }

  trigger.addEventListener('click', function (e) {
    e.stopPropagation();
    panel.hidden ? open() : close();
  });

  // On the film page, reveal the showtimes for every cinema in the selected
  // cinema's city. No-op on pages without a showtime list (e.g. the homepage),
  // so the picker behaves identically everywhere.
  function filterCinemasByCity(city) {
    const list = document.getElementById('cinemaList');
    if (!list) return;
    const prompt = document.getElementById('cinemaPrompt');
    let any = false;
    list.querySelectorAll('.cinema-block').forEach(function (block) {
      const match = block.dataset.city === city;
      block.style.display = match ? '' : 'none';
      if (match) any = true;
    });
    if (prompt) prompt.style.display = any ? 'none' : '';
  }

  // Show the chosen cinema in the bar and filter the showtimes to its city.
  function applySelection(name, city) {
    value.textContent = name;
    value.classList.remove('cs-placeholder');
    filterCinemasByCity(city);
  }

  // Remember the chosen cinema so the same selection carries across pages: pick
  // one on the homepage and the film page opens on its city, and vice versa.
  function remember(name, city) {
    try { localStorage.setItem(STORE_KEY, JSON.stringify({ name: name, city: city })); } catch (e) { /* ignore */ }
  }
  function recall() {
    try { return JSON.parse(localStorage.getItem(STORE_KEY)); } catch (e) { return null; }
  }

  select.querySelectorAll('.cs-option').forEach(function (opt) {
    opt.addEventListener('click', function () {
      applySelection(opt.dataset.value, opt.dataset.city);
      remember(opt.dataset.value, opt.dataset.city);
      close();
    });
  });

  // On load, restore the previously chosen cinema; otherwise fall back to this
  // page's default cinema, if it declares one (the film page does).
  const saved = recall();
  if (saved && saved.city) {
    applySelection(saved.name, saved.city);
  } else if (select.dataset.defaultCity) {
    filterCinemasByCity(select.dataset.defaultCity);
  }

  document.addEventListener('click', function (e) {
    if (!select.contains(e.target)) close();
  });
  document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') close();
  });
})();

// Adds prev/next scroll buttons to each movie carousel and keeps the
// scroll position in sync (arrows hide at the ends).
(function () {
  const CHEVRON_L = '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><polyline points="15 18 9 12 15 6"/></svg>';
  const CHEVRON_R = '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><polyline points="9 18 15 12 9 6"/></svg>';

  document.querySelectorAll('.carousel').forEach(function (carousel) {
    // Wrap the carousel so buttons can be absolutely positioned around it.
    const wrap = document.createElement('div');
    wrap.className = 'carousel-wrap';
    carousel.parentNode.insertBefore(wrap, carousel);
    wrap.appendChild(carousel);

    const prev = document.createElement('button');
    prev.className = 'car-btn prev';
    prev.setAttribute('aria-label', 'Vorige');
    prev.innerHTML = CHEVRON_L;

    const next = document.createElement('button');
    next.className = 'car-btn next';
    next.setAttribute('aria-label', 'Volgende');
    next.innerHTML = CHEVRON_R;

    wrap.appendChild(prev);
    wrap.appendChild(next);

    function step() {
      const card = carousel.querySelector('.movie-card');
      const cardW = card ? card.getBoundingClientRect().width + 20 : 200;
      return Math.max(cardW, Math.floor(carousel.clientWidth * 0.8));
    }

    prev.addEventListener('click', function () {
      carousel.scrollBy({ left: -step(), behavior: 'smooth' });
    });
    next.addEventListener('click', function () {
      carousel.scrollBy({ left: step(), behavior: 'smooth' });
    });

    // Center the buttons on the poster (not the whole card) and toggle
    // visibility at the scroll extremes.
    function sync() {
      const poster = carousel.querySelector('.poster');
      if (poster) {
        const top = poster.getBoundingClientRect().height / 2;
        prev.style.top = top + 'px';
        next.style.top = top + 'px';
      }
      const max = carousel.scrollWidth - carousel.clientWidth - 1;
      prev.hidden = carousel.scrollLeft <= 0;
      next.hidden = carousel.scrollLeft >= max;
    }

    carousel.addEventListener('scroll', sync, { passive: true });
    window.addEventListener('resize', sync);
    // Re-sync once images have loaded (poster height may change).
    window.addEventListener('load', sync);
    sync();
  });
})();

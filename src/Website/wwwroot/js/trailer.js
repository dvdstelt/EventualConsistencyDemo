// Trailer lightbox: play the film's trailer in an overlay instead of leaving
// the site. Converts common YouTube URLs to an autoplaying embed; anything else
// is loaded as-is.
(function () {
  const box = document.getElementById('trailerLightbox');
  if (!box) return;
  const frame = document.getElementById('trailerFrame');
  const closeBtn = document.getElementById('trailerClose');

  function toEmbed(url) {
    const id =
      (url.match(/[?&]v=([\w-]{11})/) || [])[1] ||
      (url.match(/youtu\.be\/([\w-]{11})/) || [])[1] ||
      (url.match(/\/embed\/([\w-]{11})/) || [])[1];
    return id ? 'https://www.youtube.com/embed/' + id + '?autoplay=1&rel=0' : url;
  }

  function open(url) {
    frame.src = toEmbed(url);
    box.hidden = false;
    document.body.style.overflow = 'hidden';
  }
  function close() {
    box.hidden = true;
    frame.src = 'about:blank';     // stop playback (empty string would reload the page)
    document.body.style.overflow = '';
  }

  document.querySelectorAll('.fh-play[data-trailer]').forEach(function (btn) {
    btn.addEventListener('click', function () { open(btn.dataset.trailer); });
  });
  closeBtn.addEventListener('click', close);
  box.addEventListener('click', function (e) { if (e.target === box) close(); });
  document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape' && !box.hidden) close();
  });
})();

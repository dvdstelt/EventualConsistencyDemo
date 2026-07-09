// Reviews lightbox: shows the movie's reviews and lets you add your own, stored
// in localStorage under the same key/shape the standalone reviews page uses, so
// reviews are shared between the two.
(function () {
  const box = document.getElementById('reviewsLightbox');
  if (!box) return;

  const list = document.getElementById('reviewList');
  const movieId = document.getElementById('reviewMovieId').value;
  const KEY = 'popcornStoredReviews';
  const EMAIL = 'dvdstelt@gmail.com';
  const AVATAR = 'https://gravatar.com/avatar/9300c92d4bbf31c4a96d345e9712d2a6.jpg?&s=50';

  function readAll() {
    try { return JSON.parse(localStorage.getItem(KEY)) || []; } catch (e) { return []; }
  }
  function writeAll(items) {
    try { localStorage.setItem(KEY, JSON.stringify(items)); } catch (e) { /* ignore */ }
  }
  function uuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      const r = Math.random() * 16 | 0;
      return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
  }
  function fmtDate(d) {
    const p = (n) => String(n).padStart(2, '0');
    return p(d.getDate()) + '/' + p(d.getMonth() + 1) + '/' + d.getFullYear() +
      ' ' + p(d.getHours()) + ':' + p(d.getMinutes());
  }

  function renderReview(description, date, local) {
    const li = document.createElement('li');
    if (local) li.className = 'review-local';
    li.innerHTML =
      '<div class="review-avatar"><img src="' + AVATAR + '" alt=""></div>' +
      '<div class="review-body"><div class="review-text"></div><div class="review-meta"></div></div>';
    li.querySelector('.review-text').textContent = description;
    li.querySelector('.review-meta').textContent = EMAIL + ' | ' + fmtDate(new Date(date));
    list.appendChild(li);
  }

  // Render this movie's stored reviews once, after the server-rendered ones.
  readAll()
    .filter((r) => r.MovieIdentifier === movieId)
    .forEach((r) => renderReview(r.Description, r.ReviewedAt, true));

  function open() { box.hidden = false; document.body.style.overflow = 'hidden'; }
  function close() { box.hidden = true; document.body.style.overflow = ''; }

  document.querySelectorAll('[data-reviews]').forEach(function (btn) {
    btn.addEventListener('click', function (e) { e.preventDefault(); open(); });
  });
  document.getElementById('reviewsClose').addEventListener('click', close);
  box.addEventListener('click', function (e) { if (e.target === box) close(); });
  document.addEventListener('keydown', function (e) { if (e.key === 'Escape' && !box.hidden) close(); });

  document.getElementById('reviewSubmit').addEventListener('click', function () {
    const textarea = document.getElementById('reviewText');
    const text = textarea.value.trim();
    if (!text) return;

    const now = new Date();
    const items = readAll();
    items.push({ Identifier: uuid(), MovieIdentifier: movieId, ReviewedAt: now.toISOString(), Description: text });
    writeAll(items);

    renderReview(text, now, true);
    textarea.value = '';
    list.scrollTop = list.scrollHeight;
  });

  document.getElementById('reviewsClear').addEventListener('click', function (e) {
    e.preventDefault();
    localStorage.removeItem(KEY);
    list.querySelectorAll('.review-local').forEach((el) => el.remove());
  });
})();

(function () {
    "use strict";

    var list = document.getElementById('movieList');
    if (!list || typeof Sortable === 'undefined') {
        return;
    }

    var tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    var status = document.getElementById('reorderStatus');

    function flash(message, ok) {
        if (!status) {
            return;
        }
        status.textContent = message;
        status.className = ok ? 'text-success' : 'text-danger';
        setTimeout(function () { status.textContent = ''; }, 2000);
    }

    Sortable.create(list, {
        handle: '.drag-handle',
        animation: 150,
        onEnd: function () {
            var ids = Array.prototype.map.call(
                list.querySelectorAll('tr[data-id]'),
                function (row) { return row.dataset.id; });

            fetch('/Admin/Movies/Reorder', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': tokenInput ? tokenInput.value : ''
                },
                body: JSON.stringify(ids)
            }).then(function (response) {
                flash(response.ok ? 'Order saved' : 'Could not save order', response.ok);
            }).catch(function () {
                flash('Could not save order', false);
            });
        }
    });
})();

(function () {
    "use strict";

    var input = document.getElementById('posterInput');
    var image = document.getElementById('posterCropImage');
    var dataField = document.getElementById('PosterData');
    var errorField = document.getElementById('posterError');
    var form = document.getElementById('movieForm');
    if (!input || !image || !dataField || !form) {
        return;
    }

    // Match the poster dimensions used across the site.
    var POSTER_WIDTH = 638;
    var POSTER_HEIGHT = 900;
    var cropper = null;

    input.addEventListener('change', function (e) {
        var file = e.target.files && e.target.files[0];
        if (!file) {
            return;
        }

        var reader = new FileReader();
        reader.onload = function (ev) {
            image.src = ev.target.result;
            image.style.display = 'block';
            if (cropper) {
                cropper.destroy();
            }
            cropper = new Cropper(image, {
                aspectRatio: POSTER_WIDTH / POSTER_HEIGHT,
                viewMode: 1,
                autoCropArea: 1,
                background: false
            });
            if (errorField) {
                errorField.textContent = '';
            }
        };
        reader.readAsDataURL(file);
    });

    form.addEventListener('submit', function (e) {
        if (cropper) {
            var canvas = cropper.getCroppedCanvas({ width: POSTER_WIDTH, height: POSTER_HEIGHT });
            if (canvas) {
                dataField.value = canvas.toDataURL('image/jpeg', 0.9);
            }
        }

        if (form.dataset.posterRequired === 'true' && !dataField.value) {
            e.preventDefault();
            e.stopPropagation();
            if (errorField) {
                errorField.textContent = 'Please choose and crop a poster image.';
            }
            input.focus();
        }
    });
})();

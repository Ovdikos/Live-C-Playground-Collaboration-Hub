window.cropperInstance = null;

window.initCropper = (imgElementId) => {
    const image = document.getElementById(imgElementId);
    if (!image) return;

    if (window.cropperInstance) {
        window.cropperInstance.destroy();
        window.cropperInstance = null;
    }

    window.cropperInstance = new Cropper(image, {
        aspectRatio: 1,
        viewMode: 1,
        background: false,
        autoCropArea: 1,
        responsive: true,
        ready: function () {
            document.querySelectorAll('.cropper-view-box, .cropper-face').forEach(e => {
                e.style.borderRadius = '50%';
            });
        }
    });
};

window.destroyCropper = () => {
    if (window.cropperInstance) {
        window.cropperInstance.destroy();
        window.cropperInstance = null;
    }
};

window.largeBase64Buffer = ""; 

window.exportImageToBuffer = () => {
    if (!window.cropperInstance) return 0;
    const canvas = window.cropperInstance.getCroppedCanvas({
        width: 256,
        height: 256,
        imageSmoothingQuality: "high"
    });
    window.largeBase64Buffer = canvas.toDataURL("image/png");
    return window.largeBase64Buffer.length;
};

window.getBase64Chunk = (start, end) => {
    return window.largeBase64Buffer.substring(start, end);
};

window.clearBase64Buffer = () => {
    window.largeBase64Buffer = "";
};


window.focusElement = (element) => {
    if (element && element.focus) {
        element.focus();
    }
};

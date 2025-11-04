
//const notyf = new Notyf({
//    duration: 5000,
//    position: { x: 'left', y: 'top' },
//    dismissible: true,
//    types: [
//        {
//            type: 'info',
//            background: '#17a2b8',
//            icon: {
//                className: 'bi bi-info-circle-fill',
//                tagName: 'i'
//            }
//        },
//        {
//            type: 'warning',
//            background: '#ffc107',
//            icon: {
//                className: 'bi bi-exclamation-triangle-fill',
//                tagName: 'i'
//            }
//        }
//    ]
//});

//function ShowMessage(title, message, type) {
//    const htmlMessage = `<strong>${title}</strong><br>${message}`;

//    switch (type) {
//        case 'success':
//            notyf.success(htmlMessage);
//            break;
//        case 'error':
//            notyf.error(htmlMessage);
//            break;
//        case 'warning':
//            notyf.open({
//                type: 'warning',
//                message: htmlMessage
//            });
//            break;
//        case 'info':
//            notyf.open({
//                type: 'info',
//                message: htmlMessage
//            });
//            break;
//        default:
//            notyf.open({
//                type: type || 'info',
//                message: htmlMessage
//            });
//            break;
//    }
//}

function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 5000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}

document.addEventListener("DOMContentLoaded", function () {
    const fileInput = document.getElementById("avatar");
    const avatarImage = document.getElementById("avatarPreview");

    fileInput.addEventListener("change", function () {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                avatarImage.src = e.target.result;
            };
            reader.readAsDataURL(file);
        }
    });
});

$(document).ready(function () {
    var editors = $("[ckeditor]");
    if (editors.length > 0) {
        $.getScript('/MainTemplate/assets/js/ckeditor.js', function () {
            $(editors).each(function (index, value) {
                var id = $(value).attr('ckeditor');
                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                    {
                        toolbar: {
                            items: [
                                'heading',
                                '|',
                                'bold',
                                'italic',
                                'link',
                                '|',
                                'fontSize',
                                'fontColor',
                                '|',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'undo',
                                'redo',
                                'codeBlock'
                            ]
                        },
                        language: 'fa',
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/Uploader/UploadImage'
                        }

                    })
                    .then(editor => {
                        window.editor = editor;
                    }).catch(err => {
                        console.error(err);
                    });
            });
        });
    }
});

function FillPageId(pageId) {
    $('#PageId').val(pageId);
    $('#filter-form').submit();
}
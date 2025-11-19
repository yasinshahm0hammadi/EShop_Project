
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

$(function () {
    // hide all subcategories on page load
    $('[id^="sub_categories_"]').hide();

    // (optional) make sure main checkboxes are visible
    $('[main_category_checkbox]').show();

    // checkbox change handler
    $('[main_category_checkbox]').on('change', function (e) {
        var isChecked = $(this).is(':checked');
        var selectedCategoryId = $(this).attr('main_category_checkbox');
        if (isChecked) {
            $('#sub_categories_' + selectedCategoryId).slideDown(300);
        } else {
            $('#sub_categories_' + selectedCategoryId).slideUp(300);
            $('[parent-category-id="' + selectedCategoryId + '"]').prop('checked', false);
        }
    });
})

$('#add_color_button').on('click', function (e) {

    e.preventDefault();
    var colorName = $('#product_color_name_input').val();
    var colorPrice = $('#product_color_price_input').val();
    var colorCode = $('#product_color_code_input').val();

    if (colorName !== '' && colorPrice !== '' && colorCode !== '') {
        var currentColorsCount = $('#list_of_product_colors tr');
        var index = currentColorsCount.length;


        var isExistsSelectedColor = $('[color-name-hidden-input][value="' + colorName + '"]');

        if (isExistsSelectedColor.length === 0) {


            var colorNameNode = `<input type="hidden" value="${colorName}" name="ProductColors[${index}].ColorName" color-name-hidden-input="${colorName}-${colorPrice}">`;
            var colorPriceNode = `<input type="hidden" value="${colorPrice}" name="ProductColors[${index}].Price" color-price-hidden-input="${colorName}-${colorPrice}">`;
            var colorCodeNode = `<input type="hidden" value="${colorCode}" name="ProductColors[${index}].ColorCode" color-code-hidden-input="${colorName}-${colorPrice}">`;

            $('#create_product_form').append(colorNameNode);
            $('#create_product_form').append(colorPriceNode);
            $('#create_product_form').append(colorCodeNode);


            var colorTableNode = `
          <tr color-table-item="${colorName}-${colorPrice}">
          <td>${colorName}</td> 
          <td>${colorPrice}</td>  
          <td>
          <div style="border-radius: 50%; width:40px; height: 40px; border:2px solid black; background-color: ${colorCode}"></div>
          </td>
          <td> <a class="btn btn-lg text-primary" style="float: none;" title="حذف رنگ" onclick="removeProductColor('${colorName}-${colorPrice}')">
          <i class="bi bi-trash3"></i>
          </a>
          </td>
          </tr>`;
            $('#list_of_product_colors').append(colorTableNode);
            $('#product_color_name_input').val('');
            $('#product_color_price_input').val('');
            $('#product_color_code_input').val('');


        } else {
            ShowMessage('اخطار', 'رنگ وارد شده تکراری می باشد', 'warning');
            $('#product_color_name_input').val('');
            $('#product_color_price_input').val('');
            $('#product_color_code_input').val('');

            $('#product_color_name_input').val('').focus();
        }

    } else {
        ShowMessage('اخطار', 'لطفا نام رنگ و قیمت آن را به درستی وارد نمایید', 'warning');
    }

});

function removeProductColor(index) {
    $('[color-name-hidden-input="' + index + '"]').remove();
    $('[color-price-hidden-input="' + index + '"]').remove();
    $('[color-code-hidden-input="' + index + '"]').remove();

    $('[color-table-item="' + index + '"]').remove();
    reOrderProductColorHiddenInputs();
}

function reOrderProductColorHiddenInputs() {
    var hiddenColors = $('[color-name-hidden-input]');
    $.each(hiddenColors, function (index, value) {

        var hiddenColor = $(value);
        var colorId = $(value).attr('color-name-hidden-input');
        var hiddenPrice = $('[color-price-hidden-input="' + colorId + '"]');
        var hiddenCode = $('[color-code-hidden-input="' + colorId + '"]');

        $(hiddenColor).attr('name', 'ProductColors[' + index + '].ColorName');
        $(hiddenPrice).attr('name', 'ProductColors[' + index + '].Price');
        $(hiddenCode).attr('name', 'ProductColors[' + index + '].ColorCode');
    });
}

$('#add_feature_button').on('click', function (e) {

    e.preventDefault();
    var featureTitle = $('#product_feature_title_input').val();
    var featureValue = $('#product_feature_value_input').val();

    if (featureTitle !== '' && featureValue !== '') {

        var currentFeaturesCount = $('#list_of_product_features tr');
        var index = currentFeaturesCount.length;


        var isExistsSelectedFeature = $('[feature-title-hidden-input][value="' + featureTitle + '"]');

        if (isExistsSelectedFeature.lenght !== 0) {

            var featureTitleNode = `<input type="hidden" value="${featureTitle}" name="ProductFeatures[${index}].featureTitle" feature-title-hidden-input="${featureTitle}-${featureValue}">`;
            var featureValueNode = `<input type="hidden" value="${featureValue}" name="ProductFeatures[${index}].FeatureValue" feature-value-hidden-input="${featureTitle}-${featureValue}">`;

            $('#create_product_form').append(featureTitleNode);
            $('#create_product_form').append(featureValueNode);


            var featureTableNode = `
          <tr feature-table-item="${featureTitle}-${featureValue}">
          <td>${featureTitle}</td>
          <td>${featureValue}</td>
          <td> <a class="btn btn-lg text-danger" style="float: none;" title="حذف ویژگی" onclick="removeProductFeature('${featureTitle}-${featureValue}')">
          <i class="fa fa-trash"></i>
          </a>
          </td>
          </tr>`;
            $('#list_of_product_features').append(featureTableNode);
            $('#product_feature_title_input').val('');
            $('#product_feature_value_input').val('');

        } else {
            ShowMessage('اخطار', 'ویژگی وارد شده تکراری می باشد', 'warning');
            $('#product_feature_title_input').val('');
            $('#product_feature_value_input').val('');

            $('#product_feature_title_input').val('').focus();
        }

    } else {
        ShowMessage('اخطار', 'لطفا نام ویژگی و مقدار آن را به درستی وارد نمایید', 'warning');
    }

});

function removeProductFeature(index) {
    $('[feature-title-hidden-input="' + index + '"]').remove();
    $('[feature-value-hidden-input="' + index + '"]').remove();

    $('[feature-table-item="' + index + '"]').remove();
    reOrderProductFeatureHiddenInputs();
}

function reOrderProductFeatureHiddenInputs() {
    var hiddenFeatures = $('[feature-title-hidden-input]');
    $.each(hiddenFeatures, function (index, value) {

        var hiddenFeature = $(value);
        var featureId = $(value).attr('feature-title-hidden-input');
        var hiddenFeatureValue = $('[feature-value-hidden-input="' + featureId + '"]');

        $(hiddenFeature).attr('name', 'ProductFeatures[' + index + '].FeatureTitle');
        $(hiddenFeatureValue).attr('name', 'ProductFeatures[' + index + '].FeatureValue');
    });
}
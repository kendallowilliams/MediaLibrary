import HtmlControls from '../controls/html-controls';

function initialize(): void {
    const $alertModal = $(HtmlControls.Modals().AlertModal),
        $confirmModal = $(HtmlControls.Modals().ConfirmModal),
        $errorModal = $(HtmlControls.Modals().ErrorModal),
        $warningModal = $(HtmlControls.Modals().WarningModal);

    $([$alertModal, $confirmModal, $errorModal, $warningModal]).each((index, element) => {
        if ($(element).attr('data-initialized') !== 'true') {
            $(element).on('hidden.bs.modal', e => {
                const $title = $(e.currentTarget).find('.modal-title'),
                    $body = $(e.currentTarget).find('.modal-body');

                $title.html('');
                $body.html('');
            });
        }
    });

    if ($alertModal.attr('data-initialized') !== 'true') {
        $alertModal.attr('data-initialized', 'true');
    }

    if ($confirmModal.attr('data-initialized') !== 'true') {
        $confirmModal.find('[data-button="callback"]').off();
        $confirmModal.attr('data-initialized', 'true');
    }

    if ($errorModal.attr('data-initialized') !== 'true') {
        $errorModal.attr('data-initialized', 'true');
    }

    if ($warningModal.attr('data-initialized') !== 'true') {
        $warningModal.attr('data-initialized', 'true');
    }
}

export function alert(title: string, message: string, isHtml = false): void {
    const $modal = $(HtmlControls.Modals().AlertModal),
        $title = $modal.find('.modal-title'),
        $body = $modal.find('.modal-body');

    if ($modal.attr('data-initialized') !== 'true') /*then*/ initialize();

    if (isHtml) {
        $title.html(title);
        $body.html(message);
    } else {
        $title.text(title);
        $body.text(message);
    }

    $modal.modal('show');
}

export function showError(title: string, message: string): void {
    const $modal = $(HtmlControls.Modals().ErrorModal),
        $title = $modal.find('.modal-title'),
        $body = $modal.find('.modal-body');

    if ($modal.attr('data-initialized') !== 'true') /*then*/ initialize();
    $title.text(title);
    $body.text(message);
    $modal.modal('show');
}

export function showWarning(title: string, message: string): void {
    const $modal = $(HtmlControls.Modals().WarningModal),
        $title = $modal.find('.modal-title'),
        $body = $modal.find('.modal-body');

    if ($modal.attr('data-initialized') !== 'true') /*then*/ initialize();
    $title.text(title);
    $body.text(message);
    $modal.modal('show');
}

export function confirm(title: string, message: string, showYesNo: boolean, callback: () => void): void {
    const $modal = $(HtmlControls.Modals().ConfirmModal),
        $title = $modal.find('.modal-title'),
        $body = $modal.find('.modal-body'),
        $okCancelContainer = $modal.find('[data-buttons="OK/Cancel"]'),
        $yesNoContainer = $modal.find('[data-buttons="Yes/No"]'),
        $btnContainer = showYesNo ? $yesNoContainer : $okCancelContainer;

    if ($modal.attr('data-initialized') !== 'true') /*then*/ initialize();
    $title.text(title);
    $body.text(message);
    $btnContainer.find('[data-button="callback"]').off('click').on('click', () => {
        $modal.on('hide.bs.modal', () => {
            callback();
            $modal.off('hide.bs.modal');
        });
        $modal.modal('hide');
    });
    $([$okCancelContainer, $yesNoContainer]).addClass('d-none');

    if (showYesNo) /*then*/ $yesNoContainer.removeClass('d-none');
    else /*then*/ $okCancelContainer.removeClass('d-none');
    $modal.modal('show');
}
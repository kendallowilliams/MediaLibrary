export function loadTooltips(element: HTMLElement): void {
    if (element) /*then*/ $(element).find('*[data-tooltip="tooltip"]').tooltip({ trigger: 'hover' });
}

export function disposeTooltips(element: HTMLElement): void {
    if (element) /*then*/ $(element).find('*[data-tooltip="tooltip"]').tooltip('dispose');
}
import { Injectable } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { Subject } from 'rxjs';

export type FuseAlertType =
    | 'primary'
    | 'accent'
    | 'warn'
    | 'basic'
    | 'info'
    | 'success'
    | 'warning'
    | 'error';

@Injectable({
    providedIn: 'root',
})
export class FlashMessageService {
    private flashMessageSubject = new Subject<{
        type: FuseAlertType;
        text: string;
    }>();
    // eslint-disable-next-line @typescript-eslint/member-ordering
    flashMessage$ = this.flashMessageSubject.asObservable();

    constructor(private _translocoService: TranslocoService) {}

    /**
     * Displays a success flash message with the given text.
     *
     * @param text - The success message to be translated by transloco and be displayed.
     */
    success(text: string): void {
        this.showMessage('success', text);
    }

    /**
     * Displays a success flash message with the given text.
     *
     * @param text - The already translated text
     */
    ntsuccess(text: string): void {
        this.showMessageNoTranslation('success', text);
    }

    /**
     * Displays an error message.
     *
     * @param text - The error message to be translated by transloco and be displayed.
     */
    error(text: string): void {
        this.showMessage('error', text);
    }

    /**
     * Displays a error flash message with the given text.
     *
     * @param text - The already translated text
     */
    nterror(text: string): void {
        this.showMessageNoTranslation('error', text);
    }

    /**
     * Displays a info message.
     *
     * @param text - The info message to be translated by transloco and be displayed.
     */
    info(text: string): void {
        this.showMessage('info', text);
    }

    /**
     * Displays a warning message.
     *
     * @param text - The warning message to be translated by transloco and be displayed.
     */
    warning(text: string): void {
        this.showMessage('warning', text);
    }

    /**
     * Displays a warning flash message with the given text.
     *
     * @param text - The already translated text
     */
    ntwarning(text: string): void {
        this.showMessageNoTranslation('warning', text);
    }

    /**
     * Clears the current flash message by emitting an empty message with a 'basic' type.
     * This method triggers the `flashMessageSubject` observable with an object containing
     * a type of 'basic' and an empty text string.
     */
    clear(): void {
        this.flashMessageSubject.next({ type: 'basic', text: '' });
    }

    /**
     * Displays a flash message of a specified type with translated text.
     *
     * @private
     * @param type - The type of the flash message.
     * @param text - The text to be translated and displayed in the flash message.
     */
    private showMessage(type: FuseAlertType, text: string): void {
        const translatedMessage = this._translocoService.translate(text, {});
        this.flashMessageSubject.next({ type, text: translatedMessage });
    }

    /**
     * Displays a flash message of a specified type with translated text.
     *
     * @private
     * @param type - The type of the flash message.
     * @param text - The already translated text
     */
    private showMessageNoTranslation(type: FuseAlertType, text: string): void {
        this.flashMessageSubject.next({ type, text });
    }
}

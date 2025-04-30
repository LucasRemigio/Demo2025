import { Component, OnDestroy, OnInit } from '@angular/core';
import { fadeOut } from '@fuse/animations/fade';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FlashMessageService, FuseAlertType } from './flash-message.service';

@Component({
    selector: 'app-flash-message',
    templateUrl: './flash-message.component.html',
    styleUrls: ['./flash-message.component.scss'],
})
export class FlashMessageComponent implements OnInit, OnDestroy {
    flashMessages: Array<{
        type: FuseAlertType;
        text: string;
        isFadingOut: boolean;
    }> = [];
    isFadingMessages: boolean = false;

    private _unsubscribeAll: Subject<any> = new Subject<any>();

    constructor(private _flashMessageService: FlashMessageService) {}

    ngOnInit(): void {
        // Subscribe to the flash message observable
        this._flashMessageService.flashMessage$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((message) => {
                if (message.type && message.text) {
                    this.addFlashMessage(message);
                    this.startMessageFade();
                }
                if (message.text === '') {
                    // Clear all messages
                    this.flashMessages = [];
                }
            });
    }

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    private addFlashMessage(message: {
        type: FuseAlertType;
        text: string;
    }): void {
        this.flashMessages.push({
            type: message.type,
            text: message.text,
            isFadingOut: false,
        });
    }

    private startMessageFade(): void {
        // Trigger fade-out for the first message if no fade is in progress
        if (!this.isFadingMessages) {
            this.isFadingMessages = true;
            this.fadeMessages();
        }
    }

    private fadeMessages(): void {
        if (this.shouldFadeMessage()) {
            this.initiateMessageFade();
        }
    }

    private shouldFadeMessage(): boolean {
        return (
            this.flashMessages.length > 0 && !this.flashMessages[0].isFadingOut
        );
    }

    private initiateMessageFade(): void {
        // If only one message, make it longer. Else, make them disapear faster
        let fadeOutTimeout = 5000;
        if (this.flashMessages.length > 3) {
            fadeOutTimeout = 3000;
        }

        setTimeout(() => {
            this.fadeOutFirstMessage();
        }, fadeOutTimeout); // Delay before fade-out
    }

    private fadeOutFirstMessage(): void {
        this.flashMessages[0].isFadingOut = true;

        setTimeout(() => {
            this.removeFirstMessage();
        }, 500); // Match fade-out duration
    }

    private removeFirstMessage(): void {
        this.flashMessages.shift(); // Remove the first message

        if (this.flashMessages.length > 0) {
            this.fadeMessages(); // Continue with the next message
        } else {
            this.isFadingMessages = false; // Reset the fading flag when no messages are left
        }
    }
}

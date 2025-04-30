/* eslint-disable @typescript-eslint/naming-convention */
/* eslint-disable @typescript-eslint/member-ordering */
/* eslint-disable arrow-parens */
import { Injectable, NgZone } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { UserService } from 'app/core/user/user.service';
import { UserJoinedWs } from 'app/modules/filtering/filtering.types';
import { environment } from 'environments/environment';
import { env } from 'process';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class AuditHubService {
    private hubConnection: signalR.HubConnection;

    // Resolved status message
    private messageSubject = new Subject<string>();
    public messageReceived$ = this.messageSubject.asObservable();
    // Subject for concurrency updates
    private userJoinedSubject = new Subject<any>();
    public userJoined$ = this.userJoinedSubject.asObservable();
    // Subject for user exiting the concurrency
    private userExitedSubject = new Subject<any>();
    public userExited$ = this.userExitedSubject.asObservable();

    constructor(private ngZone: NgZone, private userService: UserService) {
        this.startConnection();
    }

    // Method to send a message to the hub
    public sendMessage(message: string): void {
        this.hubConnection
            .invoke('SendMessage', message)
            .catch((err) =>
                console.error('Error while sending message: ', err)
            );
    }

    // Register a callback for when a message is received
    public onReceiveMessage(callback: (message: string) => void): void {
        this.hubConnection.on('receiveMessage', (message: string) => {
            // Run the callback within Angular's NgZone to trigger change detection
            this.ngZone.run(() => {
                callback(message);
            });
        });
    }

    public onUserJoined(callback: (data: UserJoinedWs) => void): void {
        this.hubConnection.on('userJoined', (data: UserJoinedWs) => {
            this.ngZone.run(() => {
                callback(data);
            });
        });
    }

    public onUserExited(callback: (data: UserJoinedWs) => void): void {
        this.hubConnection.on('userExited', (data: UserJoinedWs) => {
            this.ngZone.run(() => {
                callback(data);
            });
        });
    }

    public joinEmailGroup(emailToken: string): void {
        const input: UserJoinedWs = {
            email_token: emailToken,
            user_email: this.userService.getLoggedUserEmail(),
            date: new Date(),
        };

        this.hubConnection
            .invoke('JoinEmailGroup', JSON.stringify(input))
            .catch((err) =>
                console.error('Error while joining email group: ', err)
            );
    }

    public leaveEmailGroup(emailToken: string): void {
        const payload: UserJoinedWs = {
            email_token: emailToken,
            user_email: this.userService.getLoggedUserEmail(),
            date: new Date(),
        };

        this.hubConnection
            .invoke('LeaveEmailGroup', JSON.stringify(payload))
            .catch((err) =>
                console.error('Error while leaving email group: ', err)
            );
    }

    private startConnection(): void {
        const endpoint = '/ws/auditHub';
        const connectionString = environment.currrentBaseURL + '/ws/auditHub';
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(connectionString)
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.onReceiveMessage((message) => {
            this.messageSubject.next(message);
        });

        this.onUserJoined((data) => {
            this.userJoinedSubject.next(data);
        });

        this.onUserExited((data) => {
            this.userExitedSubject.next(data);
        });

        this.hubConnection
            .start()
            .then(() => {
                /*nothing*/
            })
            .catch((err) =>
                console.error('Error connecting to AuditHub: ', err)
            );
    }
}

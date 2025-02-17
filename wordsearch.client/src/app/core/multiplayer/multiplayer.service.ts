import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MultiplayerService {
  private hubConnection: signalR.HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/hubs/multiplayer')
      .build();
  }

  startConnection(): Observable<void> {
    return new Observable((observer) => {
      this.hubConnection
        .start()
        .then(() => {
          // Connection Established
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          console.error(
            'An error occurred when connecting to the multiplayer hub: ',
            error,
          );
          observer.error(error);
        });
    });
  }

  receiveMessage(): Observable<string> {
    return new Observable<string>((observer) => {
      this.hubConnection.on('ReceiveMessage', (message: string) => {
        observer.next(message);
      });
    });
  }

  sendMessage(message: string): void {
    this.hubConnection.invoke('SendMessage', message);
  }
}

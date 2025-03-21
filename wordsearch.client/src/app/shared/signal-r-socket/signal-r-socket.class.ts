import * as signalR from '@microsoft/signalr';
import { Observable, ReplaySubject, Subject } from 'rxjs';

class SignalRSocket {
  private hubConnection: signalR.HubConnection;

  constructor(url: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(url)
      .build();
  }

  startConnection(): ReplaySubject<void> {
    const connectionObserver = new Observable<void>((subscriber) => {
      if (this.hubConnection === null)
        throw new Error('Hub connection has not been initiated.');

      this.hubConnection
        .start()
        .then(() => {
          // Connection Established
          subscriber.next();
          subscriber.complete();
        })
        .catch((error) => {
          console.error(
            'An error occurred when connecting to the multiplayer hub: ',
            error,
          );
          subscriber.error(error);
        });
    });

    const replayableSubject = new ReplaySubject<void>();
    connectionObserver.subscribe(replayableSubject);
    return replayableSubject;
  }

  registerOnMethod<T extends Array<any>>(
    methodName: string,
    ...subscription: ((data: T) => void)[]
  ): Observable<T> {
    const methodObserve = new Observable<T>((observer) => {
      if (this.hubConnection === null)
        throw new Error('Hub connection has not been initiated.');

      this.hubConnection.on(methodName, (...data: T) => {
        observer.next(data);
      });
    });

    const subject = new Subject<T>();
    methodObserve.subscribe(subject)

    subscription.forEach((callback) => {
      subject.subscribe(callback);
    });
    
    return subject;
  }

  invoke(methodName: string, ...args: any[]) {
    if (!this.hubConnection)
      throw new Error('Hub connection has not been initialized');

    return this.hubConnection.invoke(methodName, ...args);
  }

  getConnectionState(): signalR.HubConnectionState {
    return this.hubConnection.state;
  }
}

export default SignalRSocket;

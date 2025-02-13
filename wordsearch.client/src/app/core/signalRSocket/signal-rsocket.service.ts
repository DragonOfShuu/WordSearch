import { Injectable } from '@angular/core';
import signalR from "@microsoft/signalr";
import { Observable } from 'rxjs';

@Injectable()
export class SignalRSocketService {
  private hubConnection: signalR.HubConnection|null = null;

  constructor() { }
  
  startConnection(url: string): Observable<void> {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(url)
      .build();

    return new Observable((observer) => {
      if (this.hubConnection === null)
        throw new Error("Hub connection has not been initiated.")

      this.hubConnection
        .start()
        .then(() => {
          // Connection Established
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          console.error("An error occurred when connecting to the multiplayer hub: ", error);
          observer.error(error)
        })
    })
  }

  registerOnMethod<T extends Array<any>>(methodName: string, ...subscription: ((data: T) => void)[]): Observable<T> {
    const methodObserve = new Observable<T>((observer) => {
      if (this.hubConnection === null)
        throw new Error("Hub connection has not been initiated.")

      this.hubConnection.on(methodName, (...data: T) => {
        observer.next(data);
      })
    })

    subscription.forEach((callback) => {
      methodObserve.subscribe(callback)
    })
    return methodObserve;
  }

  invoke(methodName: string, ...args: any[]) {
    if (!this.hubConnection)
      throw new Error("Hub connection has not been initialized")

    return this.hubConnection.invoke(methodName, ...args)
  }
}

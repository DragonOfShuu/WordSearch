import { Component, inject, OnInit } from '@angular/core';
import { MultiplayerService } from '../../core/multiplayer/multiplayer.service';

type LoadingState = 
  | 'AwaitConnection'
  | 'Connected'
  | 'Failed'

@Component({
  selector: 'shuu-multiplayer-messager',
  standalone: false,
  
  templateUrl: './multiplayer-messager.component.html',
  styleUrl: './multiplayer-messager.component.sass'
})
export class MultiplayerMessagerComponent implements OnInit {
  private multiplayerService = inject(MultiplayerService)
  error: Error|null = null;
  loadingState: LoadingState = 'AwaitConnection'
  messages: string[] = []

  constructor() {}
  
  ngOnInit(): void {
    this.multiplayerService.startConnection()
      .subscribe({
        complete: () => {
          this.loadingState = 'Connected';
  
          this.multiplayerService.sendMessage("User Connected")
          this.multiplayerService.receiveMessage().subscribe(this.receiveMessage)
        },
        error: (error) => {
          this.loadingState = 'Failed';
          this.error = error;
        }
      })
  }

  private receiveMessage(message: string) {
    this.messages.push(message)
  }

  sendMessage(message: string) {
    this.multiplayerService.sendMessage(message)
  }
}

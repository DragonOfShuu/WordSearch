import {
  Component,
  inject,
  OnInit,
  signal,
  WritableSignal,
} from '@angular/core';
import { MultiplayerService } from '../../core/multiplayer/multiplayer.service';

type LoadingState = 'AwaitConnection' | 'Connected' | 'Failed';

@Component({
  selector: 'shuu-multiplayer-messager',
  standalone: false,

  templateUrl: './multiplayer-messager.component.html',
  styleUrl: './multiplayer-messager.component.sass',
})
export class MultiplayerMessagerComponent implements OnInit {
  private multiplayerService = inject(MultiplayerService);
  error: Error | null = null;
  loadingState: LoadingState = 'AwaitConnection';
  messages: WritableSignal<string[]> = signal([]);

  constructor() {}

  ngOnInit(): void {
    this.multiplayerService.startConnection().subscribe({
      complete: () => {
        this.loadingState = 'Connected';

        console.log('Multiplayer connected!');
        this.multiplayerService
          .receiveMessage()
          .subscribe((message) => this.receiveMessage(message));
        this.multiplayerService.sendMessage('User Connected');
      },
      error: (error) => {
        this.loadingState = 'Failed';
        this.error = error;
      },
    });
  }

  receiveMessage(message: string) {
    this.messages.update((collection) => [message, ...collection]);
  }

  sendMessage(message: string) {
    console.log(`Sending message: ${message}`);
    this.multiplayerService.sendMessage(message);
  }
}

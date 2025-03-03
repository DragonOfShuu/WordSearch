import { Component, inject } from '@angular/core';
import { SingleplayerService } from '../../core/singleplayer/singleplayer.service';

@Component({
  selector: 'shuu-singleplayer-comms',
  providers: [SingleplayerService],
  templateUrl: './singleplayer-comms.component.html',
  styleUrl: './singleplayer-comms.component.sass',
})
export class SingleplayerCommsComponent {
  singleplayerService = inject(SingleplayerService);

  activationPress() {
    this.singleplayerService.newGame({
      time: 300,
      intensity: 'medium',
      level: 1,
      size: { x: 5, y: 5 },
    });
  }
}

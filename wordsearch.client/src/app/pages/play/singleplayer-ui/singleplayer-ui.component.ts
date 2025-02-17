import { Component, inject } from '@angular/core';
import { SingleplayerService } from '../../../core/singleplayer/singleplayer.service';

@Component({
  selector: 'shuu-singleplayer-ui',
  standalone: false,
  providers: [SingleplayerService],
  templateUrl: './singleplayer-ui.component.html',
  styleUrl: './singleplayer-ui.component.sass'
})
export class SingleplayerUiComponent {
  singleplayerService = inject(SingleplayerService);
}

import { Component, inject } from '@angular/core';
import { SingleplayerUiComponent } from './singleplayer-ui/singleplayer-ui.component';

@Component({
  selector: 'shuu-play',
  templateUrl: './play.component.html',
  styleUrl: './play.component.sass',
  imports: [SingleplayerUiComponent],
})
export class PlayComponent {}

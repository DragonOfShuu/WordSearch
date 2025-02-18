import { Component, computed, inject, input, OnInit } from '@angular/core';
import { SingleplayerService } from '../../../core/singleplayer/singleplayer.service';

@Component({
  selector: 'shuu-singleplayer-ui',
  standalone: false,
  providers: [SingleplayerService],
  templateUrl: './singleplayer-ui.component.html',
  styleUrl: './singleplayer-ui.component.sass',
})
export class SingleplayerUiComponent implements OnInit {
  singleplayerService = inject(SingleplayerService);
  leaveFunction = input<() => void>();
  currentBoard = this.singleplayerService.currentBoard
  boardCharacters = computed(() => this.currentBoard()?.boardCharacters ?? Array(5).fill(Array(5).fill('A')))

  ngOnInit(): void {
    // Cause ✨ JavaScript ✨
    const betterThis = this; 
    this.singleplayerService.connectionObservaboo.subscribe({
      complete() {
        betterThis.singleplayerService.newGame({intensity: 'medium', level: 1, size: {x: 5, y: 5}, time: 300})
      },
    })
  }
}

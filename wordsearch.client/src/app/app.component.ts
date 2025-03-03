import { Component } from '@angular/core';
import { NavBarComponent } from './navigation/nav-bar/nav-bar.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'shuu-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [NavBarComponent, RouterOutlet],
})
export class AppComponent {
  title = 'wordsearch.client';
}

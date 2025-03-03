import { Component, inject } from '@angular/core';
import { ThemeService } from '../../core/theme/theme.service';

@Component({
  selector: 'shuu-theme-toggle',
  templateUrl: './theme-toggle.component.html',
  styleUrl: './theme-toggle.component.sass',
})
export class ThemeToggleComponent {
  _theme = inject(ThemeService);

  constructor() {}

  themeToggle() {
    this._theme.themeData.update((old) => {
      if (old === null) return old;
      return {
        ...old,
        dark: !old?.dark,
      };
    });
  }
}

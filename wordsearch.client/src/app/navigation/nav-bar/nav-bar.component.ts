import { Component } from '@angular/core';
import { ThemeToggleComponent } from '../../shared/theme-toggle/theme-toggle.component';

type NavLink = {
  name: string;
  href: string;
};

@Component({
  selector: 'shuu-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.sass',
  imports: [ThemeToggleComponent],
})
export class NavBarComponent {
  navLinks: NavLink[] = [
    {
      name: `play`,
      href: `/`,
    },
    {
      name: 'how to',
      href: '/how-to',
    },
  ];
}

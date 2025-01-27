import { Component } from '@angular/core';

type NavLink = {
  name: string,
  href: string,
}

@Component({
  selector: 'shuu-nav-bar',
  standalone: false,
  
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.sass'
})
export class NavBarComponent {
  navLinks: NavLink[] = [
    {
      name: `play`,
      href: `/`
    },
    {
      name: 'how to',
      href: '/how-to'
    }
  ]
}

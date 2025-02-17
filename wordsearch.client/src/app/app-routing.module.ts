import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HowToComponent } from './pages/howto/how-to.component';
import { PlayComponent } from './pages/play/play.component';

const routes: Routes = [
  {
    path: '',
    component: PlayComponent,
  },
  {
    path: 'how-to',
    component: HowToComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}

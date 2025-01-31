import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HowToComponent } from './pages/howto/how-to.component';
import { ForecastTestComponent } from './test-components/forecast-test/forecast-test.component';
import { NavBarComponent } from './navigation/nav-bar/nav-bar.component';
import { MultiplayerMessagerComponent } from './test-components/multiplayer-messager/multiplayer-messager.component';
import { PlayComponent } from './pages/play/play.component';
import { ThemeToggleComponent } from './shared/theme-toggle/theme-toggle.component';
import { WordsearchComponent } from './core/wordsearch-box/wordsearch/wordsearch.component';

@NgModule({
  declarations: [
    AppComponent,
    HowToComponent,
    ForecastTestComponent,
    NavBarComponent,
    MultiplayerMessagerComponent,
    PlayComponent,
    ThemeToggleComponent,
    WordsearchComponent,
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

import { inject, Injectable, OnInit, signal, WritableSignal, effect, model } from '@angular/core';
import { BrowserStorageService } from '../../shared/browser-storage/browser-storage.service';

type ThemeBrowserData = {
  dark: boolean,
  colorScheme: 'classicGreen'
}

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  _browserStorage = inject(BrowserStorageService)
  themeData: WritableSignal<ThemeBrowserData|null> = signal(null);
  readonly THEME_KEY: string = "THEME"

  constructor() { 
    effect((clean) => {
      const writableThemeData = this.themeData()
      
      if (!writableThemeData) return;

      this._browserStorage.setData<ThemeBrowserData>(this.THEME_KEY, writableThemeData);
      const docElClassList = document.documentElement.classList

      if (writableThemeData.dark)
        docElClassList.add('dark')
      else
        docElClassList.remove('dark')
    })

    const themeData = this._browserStorage.getData<Partial<ThemeBrowserData>>(this.THEME_KEY)
  
    this.themeData.update(() => this.addMissingData(themeData||{}))
  }

  addMissingData(oldData: Partial<ThemeBrowserData>): ThemeBrowserData {
    const newData: Partial<ThemeBrowserData> = oldData;

    if (oldData.dark===undefined)
      newData.dark = false;
    if (oldData.colorScheme===undefined)
      newData.colorScheme = "classicGreen"

    return newData as ThemeBrowserData
  }
}

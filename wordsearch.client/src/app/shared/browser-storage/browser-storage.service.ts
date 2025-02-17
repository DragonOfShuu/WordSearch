import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BrowserStorageService {
  constructor() {}

  getData<T>(key: string): T | null {
    const rawData = localStorage.getItem(key);
    if (rawData === null) return null;
    return JSON.parse(rawData);
  }

  setData<T>(key: string, data: T) {
    localStorage.setItem(key, JSON.stringify(data));
  }
}

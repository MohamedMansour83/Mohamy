import { Injectable } from '@angular/core';

@Injectable()
export class LocalStorageService {

  constructor() { }

  // Stores data to local storage (not secure, doesn't expire).
  public storeToLocalStorage(key : string, obj : any) : void {
    localStorage.setItem(key,obj);
  }

  // Retrieve stored items from local storage, if not found it will return null.
  public retrieveFromLocalStorage(key : string) : any {
    return localStorage.getItem(key);
  }

  // Checks if key exists
  public checkIfKeyExist(key : string) : boolean {
    let item = localStorage.getItem(key);
    if (item != null) return true;
    return false;
  }

  // Removes key if exists from local storage
  public removeKeyFromLocalStorage(key : string) : void {
    localStorage.removeItem(key);
  }




}

import { Injectable } from '@angular/core';
import {
  LocalStorageService,
  SessionStorageService,
  LocalStorage,
  SessionStorage
} from 'ngx-webstorage';
import * as moment from 'moment';

@Injectable()
export class AppLocalStorageService {

  private cacheTimeIdentifier: string = "CacheTime";

  public isLocalAvailable: boolean;
  public isSessionAvailable: boolean;

  /*
    //default uses property name
    @LocalStorage()
    public boundAttribute;

    @SessionStorage('AnotherBoundAttribute')
    public randomName;
  */

  constructor(
    private localStorage: LocalStorageService,
    private sessionStorage: SessionStorageService,
  ) { }

  ngOnInit() {
    this.isLocalAvailable = this.localStorage.isStorageAvailable();
    this.isSessionAvailable = this.sessionStorage.isStorageAvailable();
  }

  //Local
  saveLocalValue(key, value) {
    this.localStorage.store(key, value);
    this.localStorage.store(
      key + this.cacheTimeIdentifier,
      new Date());
  }

  getLocalValue(key) {
    return this.localStorage.retrieve(key);
  }

  deleteLocalValue(key) {
    this.localStorage.clear(key);
    this.localStorage.clear(key + this.cacheTimeIdentifier);
  }

  observeLocalValue(key) {
    return this.localStorage.observe(key);
    //.subscribe((value) => console.log('new value', value));
  }

  getLocalCacheAge(key) {
    let currentTime = moment();
    let originalTime =
      moment(this.getLocalValue(key + this.cacheTimeIdentifier));
    return currentTime.diff(originalTime, 'minutes');
  }


  //Session
  saveSessionValue(key, value) {
    this.sessionStorage.store(key, value);
    this.sessionStorage.store(
      key + this.cacheTimeIdentifier,
      new Date());
  }

  getSessionValue(key) {
    return this.sessionStorage.retrieve(key);
  }

  deleteSessionValue(key) {
    this.sessionStorage.clear(key);
    this.sessionStorage.clear(key + this.cacheTimeIdentifier);
  }

  observeSessionValue(key) {
    return this.sessionStorage.observe(key);
    //.subscribe((value) => console.log('new value', value));
  }

  getSessionCacheAge(key) {
    let currentTime = moment();
    let originalTime =
      moment(this.getSessionValue(key + this.cacheTimeIdentifier));
    return currentTime.diff(originalTime, 'minutes');
  }

}

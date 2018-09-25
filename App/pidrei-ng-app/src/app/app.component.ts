import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { Device } from 'src/shared/classes/device';
import { AppPage } from './app-page';
import { Router, NavigationEnd } from '@angular/router';
import { MatIconRegistry } from '@angular/material';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public pages: AppPage[] = [];
  public activePage: AppPage;
  public deviceList: Device[] = [];
  private getDefaultPage(uri: string): AppPage {
    return {
      icon: 'unknown',
      order: 0,
      routerLink: '404',
      title: `Fehler: unbekannte Seite ${uri}`
    };
  }


  constructor(private _router: Router, iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) {

    this.pages.push({ order: 1, title: 'Heizungen', routerLink: 'heating', icon: 'heating' });
    this.pages.push({ order: 2, title: 'Wetter', routerLink: 'weather', icon: 'weather' });
    this.pages.push({ order: 3, title: 'Licht', routerLink: 'light', icon: 'light' });
    this.pages = this.pages.sort((a, b) => a.order > b.order ? 1 : -1);
    iconRegistry.addSvgIcon('heating', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/heating.svg'));
    iconRegistry.addSvgIcon('weather', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/weather.svg'));
    iconRegistry.addSvgIcon('light', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/lights.svg'));

    _router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.setActivePage(event.urlAfterRedirects);
      }
    });
  }

  private setActivePage(uri: string): void {
    if (uri.startsWith('/')) {
      uri = uri.substring(1);
    }
    this.activePage = this.pages.find(p => p.routerLink === uri) || this.getDefaultPage(uri);
  }

  ngOnInit(): void {
    this.setActivePage(this._router.url);
  }

}

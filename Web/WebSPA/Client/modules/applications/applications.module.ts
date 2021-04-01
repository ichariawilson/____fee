import { NgModule }             from '@angular/core';
import { BrowserModule  }       from '@angular/platform-browser';

import { SharedModule }         from '../shared/shared.module';
import { ApplicationsComponent } from './applications.component';
import { ApplicationsDetailComponent } from './applications-detail/applications-detail.component';
import { ApplicationsNewComponent } from './applications-new/applications-new.component';
import { ApplicationsService } from './applications.service';
import { BasketService } from '../basket/basket.service';
import { Header }                from '../shared/components/header/header';

@NgModule({
    imports: [BrowserModule, SharedModule],
    declarations: [ApplicationsComponent, ApplicationsDetailComponent, ApplicationsNewComponent],
    providers: [ApplicationsService, BasketService]
})
export class ApplicationsModule { }
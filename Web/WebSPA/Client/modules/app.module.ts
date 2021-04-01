import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from "@angular/common/http";

import { routing } from './app.routes';
import { AppService } from './app.service';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { ScholarshipModule } from './scholarship/scholarship.module';
import { ApplicationsModule } from './applications/applications.module';
import { BasketModule } from './basket/basket.module';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserAnimationsModule,
        BrowserModule,
        ToastrModule.forRoot(),
        routing,
        HttpClientModule,
        // Only module that app module loads
        SharedModule.forRoot(),
        ScholarshipModule,
        ApplicationsModule,
        BasketModule
    ],
    providers: [
        AppService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }

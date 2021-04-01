import { NgModule }             from '@angular/core';
import { BrowserModule  }       from '@angular/platform-browser';
import { CommonModule }         from '@angular/common'
import { SharedModule }         from '../shared/shared.module';
import { ScholarshipComponent }     from './scholarship.component';
import { ScholarshipService }       from './scholarship.service';
import { Pager }                from '../shared/components/pager/pager';

@NgModule({
    imports: [BrowserModule, SharedModule, CommonModule],
    declarations: [ScholarshipComponent],
    providers: [ScholarshipService]
})
export class ScholarshipModule { }

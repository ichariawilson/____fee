import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { ApplicationsService } from '../applications.service';
import { BasketService } from '../../basket/basket.service';
import { IApplication } from '../../shared/models/application.model';
import { BasketWrapperService }                     from '../../shared/services/basket.wrapper.service';

import { FormGroup, FormBuilder, Validators  }      from '@angular/forms';
import { Router }                                   from '@angular/router';

@Component({
    selector: 'f-applications_new',
    styleUrls: ['./applications-new.component.scss'],
    templateUrl: './applications-new.component.html'
})
export class ApplicationsNewComponent implements OnInit {
    newApplicationForm: FormGroup;  // new application form
    isApplicationProcessing: boolean;
    errorReceived: boolean;
    application: IApplication;

    constructor(private applicationService: ApplicationsService, private basketService: BasketService, fb: FormBuilder, private router: Router) {
        // Obtain user profile information
        this.application = applicationService.mapApplicationAndIdentityInfoNewApplication();
        this.newApplicationForm = fb.group({
            'idnumber': [this.application.idnumber, Validators.required],
            'request': [this.application.request, Validators.required],
        });
    }

    ngOnInit() {
    }

    submitForm(value: any) {
        this.application.idnumber = this.newApplicationForm.controls['idnumber'].value;
        this.application.request = this.newApplicationForm.controls['request'].value;
        this.application.paymenttypeid = 1;
        let basketCheckout = this.basketService.mapBasketInfoCheckout(this.application);
        this.basketService.setBasketCheckout(basketCheckout)
            .pipe(catchError((errMessage) => {
                this.errorReceived = true;
                this.isApplicationProcessing = false;
                return Observable.throw(errMessage); 
            }))
            .subscribe(res => {
                this.router.navigate(['applications']);
            });
        this.errorReceived = false;
        this.isApplicationProcessing = true;
    }
}
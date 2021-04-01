import { Injectable } from '@angular/core';

import { DataService } from '../shared/services/data.service';
import { IApplication } from '../shared/models/application.model';
import { IApplicationItem } from '../shared/models/applicationItem.model';
import { IApplicationDetail } from "../shared/models/application-detail.model";
import { SecurityService } from '../shared/services/security.service';
import { ConfigurationService } from '../shared/services/configuration.service';
import { BasketWrapperService } from '../shared/services/basket.wrapper.service';

import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class ApplicationsService {
    private applicationsUrl: string = '';

    constructor(private service: DataService, private basketService: BasketWrapperService, private identityService: SecurityService, private configurationService: ConfigurationService) {
        if (this.configurationService.isReady)
            this.applicationsUrl = this.configurationService.serverSettings.applyUrl;
        else
            this.configurationService.settingsLoaded$.subscribe(x => this.applicationsUrl = this.configurationService.serverSettings.applyUrl);

    }

    getApplications(): Observable<IApplication[]> {
        let url = this.applicationsUrl + '/a/api/v1/applications';

        return this.service.get(url).pipe<IApplication[]>(tap((response: any) => {
            return response;
        }));
    }

    getApplication(id: number): Observable<IApplicationDetail> {
        let url = this.applicationsUrl + '/a/api/v1/applications/' + id;

        return this.service.get(url).pipe<IApplicationDetail>(tap((response: any) => {
            return response;
        }));
    }

    mapApplicationAndIdentityInfoNewApplication(): IApplication {
        let application = <IApplication>{};
        let basket = this.basketService.basket;
        let identityInfo = this.identityService.UserData;

        console.log(basket);
        console.log(identityInfo);

        // Identity data mapping:
        application.idnumber = identityInfo.profile_idnumber;
        application.request = identityInfo.profile_request;
        application.paymenttypeid = identityInfo.payment_type;
        application.total = 0;

        // basket data mapping:
        application.applicationItems = new Array<IApplicationItem>();
        basket.items.forEach(x => {
            let item: IApplicationItem = <IApplicationItem>{};
            item.pictureurl = x.pictureUrl;
            item.scholarshipitemId = +x.scholarshipitemId;
            item.scholarshipitemname = x.scholarshipitemName;
            item.slotamount = x.slotAmount;
            item.slots = x.slots;

            application.total += (item.slotamount * item.slots);

            application.applicationItems.push(item);
        });

        application.student = basket.studentId;

        return application;
    }
}
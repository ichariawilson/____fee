import { Component, OnInit }    from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { catchError }           from 'rxjs/operators';

import { ScholarshipService }       from './scholarship.service';
import { ConfigurationService } from '../shared/services/configuration.service';
import { IScholarship }             from '../shared/models/scholarship.model';
import { IScholarshipItem }         from '../shared/models/scholarshipItem.model';
import { IScholarshipEducationLevel } from '../shared/models/scholarshipEducationLevel.model';
import { IScholarshipLocation }        from '../shared/models/scholarshipLocation.model';
import { IPager }               from '../shared/models/pager.model';
import { BasketWrapperService}  from '../shared/services/basket.wrapper.service';
import { SecurityService }      from '../shared/services/security.service';

@Component({
    selector: 'f-scholarship .f-scholarship',
    styleUrls: ['./scholarship.component.scss'],
    templateUrl: './scholarship.component.html'
})
export class ScholarshipComponent implements OnInit {
    locations: IScholarshipLocation[];
    educationlevels: IScholarshipEducationLevel[];
    scholarship: IScholarship;
    locationSelected: number;
    educationlevelSelected: number;
    paginationInfo: IPager;
    authenticated: boolean = false;
    authSubscription: Subscription;
    errorReceived: boolean;

    constructor(private service: ScholarshipService, private basketService: BasketWrapperService, private configurationService: ConfigurationService, private securityService: SecurityService) {
        this.authenticated = securityService.IsAuthorized;
    }

    ngOnInit() {

        // Configuration Settings:
        if (this.configurationService.isReady) 
            this.loadData();
        else
            this.configurationService.settingsLoaded$.subscribe(x => {
                this.loadData();
            });

        // Subscribe to login and logout observable
        this.authSubscription = this.securityService.authenticationChallenge$.subscribe(res => {
            this.authenticated = res;
        });
    }

    loadData() {
        this.getEducationLevels();
        this.getLocations();
        this.getScholarship(10, 0);
    }

    onFilterApplied(event: any) {
        event.preventDefault();
        
        this.locationSelected = this.locationSelected && this.locationSelected.toString() != "null" ? this.locationSelected : null;
        this.educationlevelSelected = this.educationlevelSelected && this.educationlevelSelected.toString() != "null" ? this.educationlevelSelected : null;
        this.paginationInfo.actualPage = 0;
        this.getScholarship(this.paginationInfo.itemsPage, this.paginationInfo.actualPage, this.locationSelected, this.educationlevelSelected);
    }

    onLocationFilterChanged(event: any, value: number) {
        event.preventDefault();
        this.locationSelected = value;
    }

    onEducationLevelFilterChanged(event: any, value: number) {
        event.preventDefault();
        this.educationlevelSelected = value;
    }

    onPageChanged(value: any) {
        console.log('scholarship pager event fired' + value);
        event.preventDefault();
        this.paginationInfo.actualPage = value;
        this.getScholarship(this.paginationInfo.itemsPage, value);
    }

    addToCart(item: IScholarshipItem) {
        this.basketService.addItemToBasket(item);
    }

    getScholarship(pageSize: number, pageIndex: number, location?: number, educationlevel?: number) {
        this.errorReceived = false;

        this.service.getScholarship(pageIndex, pageSize, location, educationlevel)
            .pipe(catchError((err) => this.handleError(err)))
            .subscribe(scholarship => {
                this.scholarship = scholarship;
                this.paginationInfo = {
                    actualPage : scholarship.pageIndex,
                    itemsPage : scholarship.pageSize,
                    totalItems : scholarship.count,
                    totalPages: Math.ceil(scholarship.count / scholarship.pageSize),
                    items: scholarship.pageSize
                };
        });
    }

    getEducationLevels() {
        this.service.getEducationLevels().subscribe(educationlevels => {
            this.educationlevels = educationlevels;
            let alleducationlevels = { id: null, educationlevel: 'All' };
            this.educationlevels.unshift(alleducationlevels);
        });
    }

    getLocations() {
        this.service.getLocations().subscribe(locations => {
            this.locations = locations;
            let alllocations = { id: null, location: 'All' };
            this.locations.unshift(alllocations);
        });
    }

    private handleError(error: any) {
        this.errorReceived = true;
        return Observable.throw(error);
    }
}
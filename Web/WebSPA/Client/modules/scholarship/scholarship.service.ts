import { Injectable } from '@angular/core';

import { DataService } from '../shared/services/data.service';
import { ConfigurationService } from '../shared/services/configuration.service';
import { IScholarship } from '../shared/models/scholarship.model';
import { IScholarshipLocation } from '../shared/models/scholarshipLocation.model';
import { IScholarshipEducationLevel } from '../shared/models/scholarshipEducationLevel.model';

import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class ScholarshipService {
    private scholarshipUrl: string = '';
    private locationUrl: string = '';
    private educationlevelsUrl: string = '';
  
    constructor(private service: DataService, private configurationService: ConfigurationService) {
        this.configurationService.settingsLoaded$.subscribe(x => {
            this.scholarshipUrl = this.configurationService.serverSettings.applyUrl + '/s/api/v1/scholarship/items';
            this.locationUrl = this.configurationService.serverSettings.applyUrl + '/s/api/v1/scholarship/scholarshiplocations';
            this.educationlevelsUrl = this.configurationService.serverSettings.applyUrl + '/s/api/v1/scholarship/scholarshipeducationlevels';
        });
    }

    getScholarship(pageIndex: number, pageSize: number, location: number, educationlevel: number): Observable<IScholarship> {
        let url = this.scholarshipUrl;

        if (educationlevel) {
            url = this.scholarshipUrl + '/educationlevel/' + educationlevel.toString() + '/location/' + ((location) ? location.toString() : '');
        }
        else if (location) {
            url = this.scholarshipUrl + '/educationlevel/all' + '/location/' + ((location) ? location.toString() : '');
        }
      
        url = url + '?pageIndex=' + pageIndex + '&pageSize=' + pageSize;

        return this.service.get(url).pipe<IScholarship>(tap((response: any) => {
            return response;
        }));
    }

    getLocations(): Observable<IScholarshipLocation[]> {
        return this.service.get(this.locationUrl).pipe<IScholarshipLocation[]>(tap((response: any) => {
            return response;
        }));
    }

    getEducationLevels(): Observable<IScholarshipEducationLevel[]> {
        return this.service.get(this.educationlevelsUrl).pipe<IScholarshipEducationLevel[]>(tap((response: any) => {
            return response;
        }));
    };
}
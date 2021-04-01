import { Component, OnInit }    from '@angular/core';
import { ApplicationsService } from './applications.service';
import { IApplication } from '../shared/models/application.model';
import { ConfigurationService } from '../shared/services/configuration.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { SignalrService } from '../shared/services/signalr.service';

@Component({
    selector: 'f-applications',
    styleUrls: ['./applications.component.scss'],
    templateUrl: './applications.component.html'
})
export class ApplicationsComponent implements OnInit {
    private oldApplications: IApplication[];
    private interval = null;
    errorReceived: boolean;

    applications: IApplication[];

    constructor(private service: ApplicationsService, private configurationService: ConfigurationService, private signalrService: SignalrService) { }

    ngOnInit() {
        if (this.configurationService.isReady) {
            this.getApplications();
        } else {
            this.configurationService.settingsLoaded$.subscribe(x => {
                this.getApplications();
            });
        }

        this.signalrService.msgReceived$
            .subscribe(x => this.getApplications());
    }

    getApplications() {
        this.errorReceived = false;
        this.service.getApplications()
            .pipe(catchError((err) => this.handleError(err)))
            .subscribe(applications => {
                this.applications = applications;
                this.oldApplications = this.applications;
                console.log('applications items retrieved: ' + applications.length);
        });
    }

    private handleError(error: any) {
        this.errorReceived = true;
        return Observable.throw(error);
    }  
}
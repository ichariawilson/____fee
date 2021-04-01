import { Component, OnInit } from '@angular/core';
import { ApplicationsService } from '../applications.service';
import { IApplicationDetail } from '../../shared/models/application-detail.model';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'f-applications_detail',
    styleUrls: ['./applications-detail.component.scss'],
    templateUrl: './applications-detail.component.html'
})
export class ApplicationsDetailComponent implements OnInit {
    public application: IApplicationDetail = <IApplicationDetail>{};

    constructor(private service: ApplicationsService, private route: ActivatedRoute) { }

    ngOnInit() {
        this.route.params.subscribe(params => {
            let id = +params['id']; // (+) converts string 'id' to a number
            this.getApplication(id);
        });
    }

    getApplication(id: number) {
        this.service.getApplication(id).subscribe(application => {
            this.application = application;
            console.log('application retrieved: ' + application.applicationnumber);
            console.log(this.application);
        });
    }
}
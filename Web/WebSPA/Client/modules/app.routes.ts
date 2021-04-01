import { Routes, RouterModule } from '@angular/router';

import { BasketComponent } from './basket/basket.component';
import { ScholarshipComponent } from './scholarship/scholarship.component';
import { ApplicationsComponent } from './applications/applications.component';
import { ApplicationsDetailComponent } from './applications/applications-detail/applications-detail.component';
import { ApplicationsNewComponent } from './applications/applications-new/applications-new.component';

export const routes: Routes = [
    { path: '', redirectTo: 'scholarship', pathMatch: 'full' },
    { path: 'basket', component: BasketComponent },
    { path: 'scholarship', component: ScholarshipComponent },
    { path: 'applications', component: ApplicationsComponent },
    { path: 'applications/:id', component: ApplicationsDetailComponent },
    { path: 'application', component: ApplicationsNewComponent },
];

export const routing = RouterModule.forRoot(routes);

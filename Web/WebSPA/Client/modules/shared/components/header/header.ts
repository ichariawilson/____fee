import { Component, Input } from '@angular/core';

@Component({
    selector: 'f-header',
    templateUrl: './header.html',
    styleUrls: ['./header.scss']
})
export class Header {
    @Input()
    url: string;
}
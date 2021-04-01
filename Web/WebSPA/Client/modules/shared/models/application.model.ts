import {IApplicationItem} from './applicationItem.model';

export interface IApplication {
    idnumber: string;
    request: number;
    paymenttypeid: number
    student: string;
    applicationnumber: string;
    total: number;
    applicationItems: IApplicationItem[];
}
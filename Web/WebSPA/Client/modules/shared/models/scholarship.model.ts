import {IScholarshipItem} from './scholarshipItem.model';

export interface IScholarship {
    pageIndex: number;
    data: IScholarshipItem[];
    pageSize: number;
    count: number;
}
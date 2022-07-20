
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { ValidationResponse } from '../api/response';


export class ValidateBackendErrors {
    static Validate(e: any, messageService: MessageService, route: Router) {
        if ((e.error as ValidationResponse).Code != undefined && e.status == 400) {
            const validation = e.error as ValidationResponse;
            const errors = validation.Errors;
            for (let key in errors) {
                let value = errors[key];
                messageService.add({ severity: 'error', summary: 'Rejected', detail: `${key}: ${value}`, life: 7000 });
            }
        }
        else{
            route.navigate(['/pages/error']);
            console.log(e);
        }
    }
}
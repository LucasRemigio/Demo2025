import { Component, Input, OnInit } from '@angular/core';
import { translate } from '@ngneat/transloco';
import { UserService } from 'app/core/user/user.service';
import { EMAIL_STATUSES } from 'app/modules/filtering/filtering.types';
import { FilteredEmail } from '../details/details.types';

@Component({
    selector: 'app-validate-cards',
    templateUrl: './validate-cards.component.html',
    styleUrls: ['./validate-cards.component.scss'],
})
export class ValidateCardsComponent implements OnInit {
    @Input() filteredEmails: FilteredEmail[] = [];
    @Input() getReasonClass!: (status: string) => string;
    @Input() getStatusDescription!: (status: string) => string;

    isUserSupervisor: boolean = false;

    constructor(private _userService: UserService) {}

    ngOnInit(): void {
        this.isUserSupervisor = this._userService.isSupervisor();
    }

    isEmailPendingAdminApproval(status: string): boolean {
        if (this.isUserSupervisor) {
            return false;
        }

        return (
            status ===
            EMAIL_STATUSES.PENDENTE_APROVACAO_ADMINISTRACAO.description
        );
    }
}

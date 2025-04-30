export interface User {
    id?: string;
    name: string;
    email?: string;
    role: string;
    avatar?: string;
    status?: string;
    departments?: Department[];
}

export interface Department {
    id: string;
    name: string;
}

export interface Password {
    OldPass: string;
    NewPass: string;
    RepeatNewPass: string;
    email: string;
}

export interface Auth2F {
    user?: string;
    auth2f?: string;
    created_at?: string;
}

export interface ValidateAuth2f {
    auth2f_code: string;
    email: string;
}

export interface SendNewAuth2fCode {
    email: string;
}

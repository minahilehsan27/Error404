// Toast Notification System
function showToast(message, type = 'info', duration = 3000) {
    const toastId = 'toast-' + Date.now();
    const toast = document.createElement('div');

    const icons = {
        success: 'fas fa-check-circle',
        error: 'fas fa-exclamation-circle',
        warning: 'fas fa-exclamation-triangle',
        info: 'fas fa-info-circle'
    };

    const colors = {
        success: '#2ecc71',
        error: '#e74c3c',
        warning: '#f39c12',
        info: '#0077b6'
    };

    toast.id = toastId;
    toast.innerHTML = `
        <div class="toast-content" style="
            background: ${colors[type] || colors.info};
            color: white;
            padding: 15px 20px;
            border-radius: 8px;
            display: flex;
            align-items: center;
            gap: 10px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.2);
            animation: slideIn 0.3s ease-out;
        ">
            <i class="${icons[type] || icons.info}"></i>
            <span>${message}</span>
        </div>
    `;

    // Add styles if not already added
    if (!document.getElementById('toast-styles')) {
        const style = document.createElement('style');
        style.id = 'toast-styles';
        style.textContent = `
            .toast-container {
                position: fixed;
                top: 20px;
                right: 20px;
                z-index: 9999;
                display: flex;
                flex-direction: column;
                gap: 10px;
            }
            
            @keyframes slideIn {
                from { transform: translateX(100%); opacity: 0; }
                to { transform: translateX(0); opacity: 1; }
            }
            
            @keyframes slideOut {
                from { transform: translateX(0); opacity: 1; }
                to { transform: translateX(100%); opacity: 0; }
            }
        `;
        document.head.appendChild(style);
    }

    // Get or create toast container
    let container = document.querySelector('.toast-container');
    if (!container) {
        container = document.createElement('div');
        container.className = 'toast-container';
        document.body.appendChild(container);
    }

    container.appendChild(toast);

    // Remove toast after duration
    setTimeout(() => {
        const toastToRemove = document.getElementById(toastId);
        if (toastToRemove) {
            toastToRemove.style.animation = 'slideOut 0.3s ease-out forwards';
            setTimeout(() => {
                if (toastToRemove.parentNode) {
                    toastToRemove.parentNode.removeChild(toastToRemove);
                }
            }, 300);
        }
    }, duration);
}

// Form validation
function validateForm(formId) {
    const form = document.getElementById(formId);
    if (!form) return true;

    const requiredFields = form.querySelectorAll('[required]');
    let isValid = true;

    requiredFields.forEach(field => {
        if (!field.value.trim()) {
            field.style.borderColor = '#e74c3c';
            isValid = false;

            // Add error message
            let errorDiv = field.nextElementSibling;
            if (!errorDiv || !errorDiv.classList.contains('error-message')) {
                errorDiv = document.createElement('div');
                errorDiv.className = 'error-message text-danger small mt-1';
                errorDiv.textContent = 'This field is required';
                field.parentNode.appendChild(errorDiv);
            }
        } else {
            field.style.borderColor = '#2ecc71';
            const errorDiv = field.nextElementSibling;
            if (errorDiv && errorDiv.classList.contains('error-message')) {
                errorDiv.remove();
            }
        }
    });

    return isValid;
}

// Password strength checker
function checkPasswordStrength(password) {
    let strength = 0;

    if (password.length >= 8) strength++;
    if (/[A-Z]/.test(password)) strength++;
    if (/[a-z]/.test(password)) strength++;
    if (/[0-9]/.test(password)) strength++;
    if (/[^A-Za-z0-9]/.test(password)) strength++;

    return strength;
}

// Booking status update
function updateBookingStatus(bookingId, newStatus) {
    if (confirm('Are you sure you want to update the status?')) {
        fetch(`/Booking/UpdateStatus/${bookingId}?status=${newStatus}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    showToast('Status updated successfully!', 'success');

                    // Update UI
                    const statusElement = document.getElementById(`status-${bookingId}`);
                    if (statusElement) {
                        statusElement.className = `status-badge status-${newStatus.toLowerCase()}`;
                        statusElement.textContent = newStatus;
                    }
                } else {
                    showToast('Failed to update status', 'error');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                showToast('Error updating status', 'error');
            });
    }
}

// Load more services (pagination)
let currentPage = 1;
function loadMoreServices() {
    currentPage++;

    fetch(`/Customer/LoadMore?page=${currentPage}`)
        .then(response => response.json())
        .then(data => {
            if (data.services.length > 0) {
                const container = document.getElementById('services-container');
                data.services.forEach(service => {
                    const serviceCard = createServiceCard(service);
                    container.appendChild(serviceCard);
                });

                if (!data.hasMore) {
                    document.getElementById('load-more-btn').style.display = 'none';
                }
            } else {
                document.getElementById('load-more-btn').style.display = 'none';
                showToast('No more services to load', 'info');
            }
        });
}

function createServiceCard(service) {
    const div = document.createElement('div');
    div.className = 'col-md-6 col-lg-4';
    div.innerHTML = `
        <div class="service-card h-100">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h5 class="mb-0">${service.title}</h5>
                <span class="badge-karigar">${service.category}</span>
            </div>
            <div class="card-body">
                <div class="d-flex align-items-center mb-3">
                    <div class="rounded-circle bg-light d-flex align-items-center justify-content-center me-3"
                         style="width: 60px; height: 60px;">
                        <i class="fas fa-user-tie fa-2x text-primary"></i>
                    </div>
                    <div>
                        <h6 class="mb-1">${service.providerName}</h6>
                        <p class="text-muted small mb-0">
                            <i class="fas fa-map-marker-alt me-1"></i> ${service.location}
                        </p>
                    </div>
                </div>
                <p class="card-text">${service.description.substring(0, 120)}...</p>
                <div class="d-flex justify-content-between align-items-center mb-3">
                    <div>
                        <span class="fw-bold text-primary fs-5">₹${service.price}</span>
                        <span class="text-muted">/service</span>
                    </div>
                    <div class="text-warning">
                        ${'<i class="fas fa-star text-warning"></i>'.repeat(Math.floor(service.rating))}
                        ${service.rating % 1 !== 0 ? '<i class="fas fa-star-half-alt text-warning"></i>' : ''}
                        ${'<i class="fas fa-star text-light"></i>'.repeat(5 - Math.ceil(service.rating))}
                        <span class="ms-1">(${service.reviewCount})</span>
                    </div>
                </div>
                <div class="d-grid">
                    <a href="/Customer/Details/${service.id}" class="btn btn-karigar-primary">
                        <i class="fas fa-calendar-plus me-2"></i> Book Now
                    </a>
                </div>
            </div>
        </div>
    `;
    return div;
}

// Initialize when document is ready
document.addEventListener('DOMContentLoaded', function () {
    // Add animation to cards
    const cards = document.querySelectorAll('.service-card, .dashboard-card');
    cards.forEach((card, index) => {
        card.style.animationDelay = `${index * 0.1}s`;
        card.classList.add('fade-in');
    });

    // Form submission handling
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            if (!validateForm(this.id)) {
                e.preventDefault();
                showToast('Please fill all required fields', 'error');
            }
        });
    });

    // Password strength indicator
    const passwordInputs = document.querySelectorAll('input[type="password"]');
    passwordInputs.forEach(input => {
        input.addEventListener('input', function () {
            const strength = checkPasswordStrength(this.value);
            const indicator = document.getElementById('password-strength');
            if (indicator) {
                indicator.textContent = ['Very Weak', 'Weak', 'Fair', 'Good', 'Strong'][strength];
                indicator.className = `badge bg-${['danger', 'danger', 'warning', 'info', 'success'][strength]}`;
            }
        });
    });

    // Auto-hide alerts after 5 seconds
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.transition = 'opacity 0.5s';
            alert.style.opacity = '0';
            setTimeout(() => alert.remove(), 500);
        }, 5000);
    });
});
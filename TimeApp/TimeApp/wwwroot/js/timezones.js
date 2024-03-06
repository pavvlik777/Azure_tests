async function getTimezonesAsync() {
    const response = await fetch('/api/time');
    const data = response.json();

    return data;
}

async function createTimezoneAsync(timezoneData) {
    const response = await fetch(`/api/time`, {
        method: "post",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(timezoneData)
    });
    const data = response.json();

    return data;
}

async function updateTimezoneAsync(timezoneId, timezoneData) {
    await fetch(`/api/time/${timezoneId}`, {
        method: "put",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(timezoneData)
    });
    const data = response.json();

    return data;
}

async function deleteTimezoneAsync(timezoneId) {
    await fetch(`/api/time/${timezoneId}`, {
        method: "delete"
    });
}

async function getDiffAsync(timezoneId) {
    const response = await fetch(`/api/time/diff/minsk/${timezoneId}`, {
        method: "post"
    });
    const data = response.json();

    return data;
}

async function changeTtlAsync(timezoneId) {
    const response = await fetch(`/api/time/${timezoneId}/ttl`, {
        method: "post"
    });
    const data = response.json();

    return data;
}

async function updateTimeAsync() {
    const date = new Date();
    const timezoneContainer = document.getElementById("timezones");
    const timezones = timezoneContainer.querySelectorAll(".timezone");
    for (const timezone of timezones) {
        const utc = date.getTime() + (date.getTimezoneOffset() * 60000);
        const utcOffsetMinutes = timezone.dataset.offset;
        const current = new Date(utc + utcOffsetMinutes * 60000);
        const timezoneTime = timezone.querySelector(".timezone__time");
        timezoneTime.textContent = current.toLocaleString();
    }

    setTimeout(updateTimeAsync, 500);
}

function createTimezoneObject(timezone) {
    const timezoneContainer = document.getElementById("timezones");
    const timezoneTemplate = timezoneContainer.querySelector("#timezone-template");

    let newTimezone = timezoneTemplate.cloneNode(true);
    timezoneTemplate.after(newTimezone);

    newTimezone.style.display = '';

    const timezoneName = newTimezone.querySelector(".timezone__name");
    timezoneName.textContent = timezone.displayName;
    newTimezone.id = 'timezone_' + timezone.zoneId;
    newTimezone.dataset.offset = timezone.utcOffsetMinutes;

    const timezoneDiffButton = newTimezone.querySelector(".timezone__diff");
    timezoneDiffButton.onclick = async () => {
        const diffContaienr = document.getElementById("diff-container");
        const diff = await getDiffAsync(timezone.zoneId);
        diffContaienr.textContent = diff / 60 + ' hours';
    };

    const timezoneTtlCheckboxLabel = newTimezone.querySelector("label.timezone__ttl");
    timezoneTtlCheckboxLabel.htmlFor = 'ttl-change_' + timezone.zoneId;

    const timezoneTtlCheckbox = newTimezone.querySelector("input.timezone__ttl");
    timezoneTtlCheckbox.id = 'ttl-change_' + timezone.zoneId;
    timezoneTtlCheckbox.name = 'ttl-change_' + timezone.zoneId;
    timezoneTtlCheckbox.onclick = async () => {
        await changeTtlAsync(timezone.zoneId);
    };
}

async function createFormSubmit(event) {
    event.preventDefault();

    const timezoneName = document.getElementById("create-form-name").value;
    const timezoneOffset = document.getElementById("create-form-offset").value;
    const timezoneData = {
        DisplayName: timezoneName,
        UtcOffsetMinutes: timezoneOffset
    };
    var response = await createTimezoneAsync(timezoneData);
    createTimezoneObject(response);

}

(async () => {
    const timezones = await getTimezonesAsync();
    for (const timezone of timezones) {
        createTimezoneObject(timezone);
    }

    await updateTimeAsync();
})();

document.getElementById("create-form").addEventListener("submit", createFormSubmit);
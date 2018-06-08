# DynamicQL
Dynamic SQL query generation is the end goal. Requests are based on GraphQL from facebook but Im looking to get out of the need for any setup or schema etc.

A json (or GraphQL) based query format will be submit as so

`{`
	`intranetpagelaouyx {`
		`table(like: 'moose')	`
	`}`

	`employee {`
		`name,`
		`surtnmae,`
		`etxar`
	`}`
`}`

And it will respond with the identified structure or the code and currenlty a dummy SQL code

```json
{
	"Result": {
		"SqlQuery": "SELECT  FROM <ROOT>",
		"Properties": [
			{
				"SqlQuery": "SELECT table(like:, 'moose') FROM intranetpagelaouyx",
				"Properties": [
					{
						"ElementType": 1,
						"Name": "table(like:"
					},
					{
						"ElementType": 1,
						"Name": "'moose')"
					}
				],
				"ElementType": 0,
				"Name": "intranetpagelaouyx"
			},
			{
				"SqlQuery": "SELECT name, surtnmae, etxar FROM employee",
				"Properties": [
					{
						"ElementType": 1,
						"Name": "name"
					},
					{
						"ElementType": 1,
						"Name": "surtnmae"
					},
					{
						"ElementType": 1,
						"Name": "etxar"
					}
				],
				"ElementType": 0,
				"Name": "employee"
			}
		],
		"ElementType": 0,
		"Name": "<ROOT>"
	},
	"Id": 115,
	"Exception": null,
	"Status": 5,
	"IsCanceled": false,
	"IsCompleted": true,
	"IsCompletedSuccessfully": true,
	"CreationOptions": 0,
	"AsyncState": null,
	"IsFaulted": false
}
```

The gradual idea being to add more processing as I go. Using the submission format of GraphQL as a guide, I want a free, scheme free GraphQL like code that will simply attempt to return what I ask for with no setup!

Systems I work with have a forever changing schema that can even be user customised... joy. This rules out GraphQL as a playground for us so this fun project should give me the options I need.

ToDos:

- [x] Read basic submissions
- [x] Identify objects and values
- [ ] Support filters
- [ ] Check and validate fields requests
- [ ] Generate linked SQL results
- [ ] Return created objects
